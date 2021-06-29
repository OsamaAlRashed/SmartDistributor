using SmartDistributor.Main.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Idata
{
    public interface IMLRepository
    {
        Task<string> GenerateSellerCsv(List<int> zipPrefixs);
        Task<string> GenerateSellerCategoryCsv();
        Task<string> GenerateSellerClusterCsv();
        Task<string> GenerateFullSellerProductCsv();
        Task<bool> EditSellersClusters(List<int> sellerclusters);
        Task<bool> FillIsChooseProduct(Guid guid);
        Task<string> GenerateFullSellerProductV2();
        Task<bool> GenerateNewProductCsv();
        Task<bool> GenerateTop2000ProductsCsv();
        Task<bool> GenerateProductsV3();
        Task<bool> GenerateProducts2000V4();
        Task<bool> GenerateAllProductsV4();
        Task<bool> GenerateFileCsvV5();
        Task<bool> FillCategoriesNumber(); 
        Task<bool> GenerateFinalCsv(); 
        Task<bool> RemoveGeoLocationAndCity(); 
        Task<bool> FilterGeoLocationAndCity(); 
        Task<bool> AddGeoLocation(GeoLocationDto geoLocationDto); 

    }
}
