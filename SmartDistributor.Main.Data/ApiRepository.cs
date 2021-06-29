using Microsoft.EntityFrameworkCore;
using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Dto.ApiDto;
using SmartDistributor.Main.Idata;
using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Data
{
    public class ApiRepository : BaseRepository , IApiRepository
    {
        public ApiRepository(SmartDistributorDbContext context) : base(context)
        {

        }

        public async Task<List<CategoryDto>> GetCategories(Guid? catId)
        {
            try
            {
                var result = Context.Categories
                                .Select(c => new CategoryDto
                                {
                                    Id = c.Id,
                                    Name = c.Name,
                                    ProductsCount = c.Products.Count()
                                }).ToList();
                return result;
            }
            catch (Exception)
            {
                return new List<CategoryDto>();
            }
            
        }

        public async Task<List<ApiCityDto>> GetCities(Guid? cityId)
        {
            List<ApiCityDto> Cities = new List<ApiCityDto>(); 
            try
            {
                Cities = await Context.Cities
                                       .Select(c => new ApiCityDto
                                       {
                                           Id = c.Id,
                                           Name = c.Name
                                       })
                                      .ToListAsync();

                return Cities;
            }
            catch (Exception)
            {
                return new List<ApiCityDto>();
            }
        }

        public async Task<List<ApiProductDto>> GetProducts(int? CatNumber)
        {
            var Products = new List<ApiProductDto>();
            try
            {
                var orderItems = Context.OrderItems
                                        .ToList()
                                        .GroupBy(o => o.ProductId)
                                        .Select(op => new
                                        {
                                            ProductId = op.Key,
                                            Price = op.First().Price,
                                            Count = op.Count()
                                        }).ToList();
                Dictionary<Guid, Tuple<double, int>> orderItemsDic = new Dictionary<Guid, Tuple<double, int>>();
                foreach (var item in orderItems)
                {
                    orderItemsDic[item.ProductId] = new Tuple<double, int>(item.Price, item.Count);
                } 

                Products = await Context.Products
                    .Include(p => p.Category)
                    .Select(product => new ApiProductDto
                    {
                        Id = product.Id,
                        CategoryNumber = product.Category.Number,
                        CategoryId = product.CategoryId,
                        Price = !orderItemsDic.ContainsKey(product.Id) ? 100 : orderItemsDic[product.Id].Item1,
                        TotalOfPaid = !orderItemsDic.ContainsKey(product.Id) ? 0 : orderItemsDic[product.Id].Item2,
                        Weight = product.Weight,
                        Height = product.Height,
                        Length = product.Length,
                        Width = product.Width
                    })
                    .Take(100)
                    .ToListAsync();
                
            }
            catch (Exception ex)
            {
                return new List<ApiProductDto>();
            }
            return Products;
        }

        public async Task<ApiProductDto> GetProductsById(Guid id)
        {
            try
            {
                

                var product = await Context.Products
                                    .Include(p => p.Category)
                                    .SingleOrDefaultAsync(p => p.Id == id);
                return new ApiProductDto
                {
                    Id = product.Id,
                    CategoryNumber = product.Category.Number,
                    CategoryId = product.CategoryId,
                    Weight = product.Weight,
                    Height = product.Height,
                    Length = product.Length,
                    Width = product.Width
                };
            }
            catch (Exception)
            {
                return new ApiProductDto();
            }
        }

        public async Task<List<ApiSellerDto>> GetSellers(List<Guid> guids)
        {
            var GeoLocations = await Context.Geolocations.ToListAsync();

            Dictionary<int, Tuple<double, double, Guid>> GeoLocationsDic = new Dictionary<int, Tuple<double, double, Guid>>();

            foreach (var geo in GeoLocations)
            {
                GeoLocationsDic[geo.ZipCodePrefix] = new Tuple<double, double, Guid>(geo.Lat, geo.Lng, geo.CityId);
            }

            var sellers = await Context.Sellers
                                       .Where(s => guids.Contains(s.Id))
                                       .Select(s => new ApiSellerDto
                                       {
                                           Id = s.Id,
                                           City = s.City,
                                           Lat = GeoLocationsDic[s.ZipCodePrefix].Item1.ToString(),
                                           Lng = GeoLocationsDic[s.ZipCodePrefix].Item2.ToString(),
                                           State = s.State
                                       })
                                       .ToListAsync();
            return sellers;
        }

        public async Task<List<ApiSellerDto>> PredicteSellers(int predction, Guid? cityId)
        {
            
            var Sellers = new List<ApiSellerDto>();
            try
            {
                var GeoLocations = await Context.Geolocations
                                                .ToListAsync();
                Dictionary<int, Tuple<double, double , Guid>> GeoLocationsDic = new Dictionary<int, Tuple<double, double, Guid>>();

                foreach (var geo in GeoLocations)
                {
                    GeoLocationsDic[geo.ZipCodePrefix] = new Tuple<double, double , Guid>(geo.Lat , geo.Lng , geo.CityId);
                }



                Sellers = await Context.Sellers
                                       .Where(s => (s.ClusterId == predction)
                                        && (cityId.HasValue ? GeoLocationsDic[s.ZipCodePrefix].Item3 == cityId : true))
                                       .Select(s => new ApiSellerDto
                                       {
                                           Id = s.Id,
                                           City = s.City,
                                           Lat = GeoLocationsDic[s.ZipCodePrefix].Item1.ToString(),
                                           Lng = GeoLocationsDic[s.ZipCodePrefix].Item2.ToString(),
                                           State = s.State
                                       })
                                       .ToListAsync();

                return Sellers;
            }
            catch (Exception ex)
            {
                return new List<ApiSellerDto>();
            }
        }

    }
}
