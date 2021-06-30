using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Dto.ApiDto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Idata
{
    public interface IApiRepository
    {
        Task<List<ApiProductDto>> GetProducts(int? CatNumber);
        Task<ApiProductDto> GetProductsById(Guid Id);
        Task<List<ApiSellerDto>> PredicteSellers(int predction , Guid? CityId);
        Task<List<ApiSellerDto>> GetSellers(List<Guid> guids);
        Task<List<ApiCityDto>> GetCities(Guid? cityId);
        Task<List<CategoryDto>> GetCategories(Guid? catId);
        Task<List<ApiProductDto>> GetTopProducts(Guid CityId);
        Task<List<ApiSellerDto>> GetTopSellers(Guid CityId);

    }
}
