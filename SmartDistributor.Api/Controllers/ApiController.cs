using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SmartDistributor.Api.Genetic_Algorithm;
using SmartDistributor.Main.Dto.ApiDto;
using SmartDistributor.Main.Idata;
using SmartDistributor.SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IApiRepository apiRepository;
        private readonly IStatisticsRepository statisticsRepository;
        private readonly InferenceSession session;

        public ApiController(IApiRepository apiRepository , IStatisticsRepository statisticsRepository , InferenceSession session)
        {
            this.apiRepository = apiRepository;
            this.statisticsRepository = statisticsRepository;
            this.session = session;
        }

        [HttpPost]
        public async Task<IActionResult> PredicteSellers(List<Guid> ProductIds, Guid? CityId)
        {
            var Sellers = new List<ApiSellerDto>();
            foreach (var ProductId in ProductIds)
            {
                var product = await apiRepository.GetProductsById(ProductId);
                int predction = Prediction(product);
                Sellers.AddRange(await apiRepository.PredicteSellers(predction, CityId));
                Sellers.DistinctBy(s => s.Id);
            }

            return Ok(Sellers);
        }

        [HttpPost]
        public async Task<IActionResult> GetBestPath(SellersPointDto sellersPoint)
        {
            var Sellers = await apiRepository.GetSellers(sellersPoint.Ids);

            var position = Sellers.Select(s => new Tuple<Guid , double, double>
                                               (s.Id , Double.Parse(s.Lat), Double.Parse(s.Lng)))
                                  .ToList();

            var startPoint = new Tuple<Guid, double, double>(Guid.NewGuid(), Double.Parse(sellersPoint.Lat), Double.Parse(sellersPoint.Lng));

            GA_TSP tsp = new GA_TSP();

            tsp.Initialization(position , startPoint);

            var orderedPosition = tsp.TSPCompute();

            List<ApiSellerDto> orderedPositionList = new List<ApiSellerDto>();

            foreach (var p in orderedPosition)
            {
                orderedPositionList.Add(new ApiSellerDto
                {
                    Id = p.Item1,
                    Lat = p.Item2.ToString(),
                    Lng = p.Item3.ToString()
                });
            }

            return Ok(orderedPositionList);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int? catNumber)
        {
            var products = await apiRepository.GetProducts(catNumber);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetCities(Guid? cityId)
        {
            var products = await apiRepository.GetCities(cityId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories(Guid catId)
        {
            var products = await apiRepository.GetCategories(catId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopProducts(Guid CityId)
        {
            var products = await apiRepository.GetTopProducts(CityId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetTopSellers(Guid CityId)
        {
            var products = await apiRepository.GetTopSellers(CityId);
            return Ok(products);
        }

        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var products = await statisticsRepository.GetCounts();
            return Ok(products);
        }

        private int Prediction(ApiProductDto product)
        {
            int[] dimensions = new int[] { 1 , 4 };
            int[] Inputs = new int[]
            {
                product.CategoryNumber , product.Height , product.Length , product.Width
            };

            var DenseTensor = new DenseTensor<int>(Inputs, dimensions);

            var xs = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor<int>("input", DenseTensor)
            };

            var result = session.Run(xs);

            List<float> score = result.First().AsEnumerable<float>().ToList();

            float max = -1;
            int ans = 0;
            for (int i = 0; i < 64; i++)
            {
                if (score[i] > max)
                {
                    max = score[i];
                    ans = i;
                }
            }

            result.Dispose();
            return ans + 1;
        }
    }
}
