using SmartDistributor.Main.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Idata
{
    public interface IStatisticsRepository
    {
        Task<CountDto> GetCounts();
        Task<IEnumerable<ProductDto>> GetBestProducts();
        Task<IEnumerable<ProductDto>> GetBestProductInCategory();

        Task<IEnumerable<object>> GetTop2000Products();
        Task<IEnumerable<object>> GetProductsCountInCategories();

        Task<string> GetProductsSellersCount();
        Task<List<SellerProductsCountDto>> GetProductCountForSellers();
        Task<int> GetProductsCountBySeller(int zipCodePrefix);
        Task<object> GetCategoryCountBySeller();
        Task<bool> EditCategoryForProduct(Guid guid);
        Task<object> GetProAndCatInState();
        Task<List<int>> GetSellerwithoutGeo();
        Task<bool> SetSellerZipPrefex(int pa , int newpa);
    }
}
