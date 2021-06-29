using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartDistributor.Main.Idata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsRepository statisticsRepository;

        public StatisticsController(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCounts()
        {
            var result = await statisticsRepository.GetCounts();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetBestProducts()
        {
            var result = await statisticsRepository.GetBestProducts();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetProductCountInState()
        {
            var result = "";
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetBestProductInCategory()
        {
            var result = await statisticsRepository.GetBestProductInCategory();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsCountInCategories()
        {
            var result = await statisticsRepository.GetProductsCountInCategories();
            return new JsonResult(result) { StatusCode = 200 };
        }


        [HttpGet]
        public async Task<IActionResult> GetTop2000Products()
        {
            var result = await statisticsRepository.GetTop2000Products();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsSellersCount()
        {
            var result = await statisticsRepository.GetProductsSellersCount();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetProductCountForSellers()
        {
            var result = await statisticsRepository.GetProductCountForSellers();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsCountBySeller(int zipPrefixCode)
        {
            var result = await statisticsRepository.GetProductsCountBySeller(zipPrefixCode);
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryCountBySeller()
        {
            var result = await statisticsRepository.GetCategoryCountBySeller();
            return new JsonResult(result) { StatusCode = 200 };
        }

        //[HttpGet]
        //public async Task<IActionResult> EditCategoryForProduct(Guid guid)
        //{
        //    var result = await statisticsRepository.EditCategoryForProduct();
        //    return new JsonResult(result) { StatusCode = 200 };
        //}

        [HttpGet]
        public async Task<IActionResult> GetProAndCatInState()
        {
            var result = await statisticsRepository.GetProAndCatInState();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetSellerwithoutGeo()
        {
            var result = await statisticsRepository.GetSellerwithoutGeo();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> SetSellerZipPrefex(int ZipPrefex , int newZip)
        {
            var result = await statisticsRepository.SetSellerZipPrefex(ZipPrefex , newZip);
            return new JsonResult(result) { StatusCode = 200 };
        }



    }
}
