using SmartDistributor.Main.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Idata
{
    public interface IMainRepository
    {
        Task<bool> AddStates(List<string> states);
        Task<bool> AddCategory(List<string> categories);
        Task<bool> AddCities(List<CityDto> cityDtos);
        Task<bool> AddProducts(List<ProductDto> productDtos);
        Task<bool> AddSellers(List<SellerDto> sellerDtos);
        Task<bool> AddCustomers(List<CustomerDto> customerDtos);
        Task<bool> AddGeoLocations(List<GeoLocationDto> geoLocationDtos);
        Task<bool> AddOrders(List<OrderDto> orderDtos);
        Task<bool> AddOrderItems(List<OrderItemDto> orderItemDtos);
    }
}
