using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;
using System.Text;
using SmartDistributor.Models.Main;
using SmartDistributor.Main.Dto;
using SmartDistributor.Api.Models;
using SmartDistributor.Main.Idata;

namespace SmartDistributor.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private readonly IMainRepository mainRepository;

        public MainController(IMainRepository mainRepository)
        {
            this.mainRepository = mainRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ReadCategory()
        {
            var result = new List<CategoryCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\category.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<CategoryCsv>().ToList();
            }
            var resultDto = result.Select(s => s.Name).ToList();
            

            var op = await mainRepository.AddCategory(resultDto);

            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadState()
        {
            var result = new List<StateCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\State.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<StateCsv>().ToList();
            }
            var resultDto = result.Select(s => s.Name).Distinct().ToList();

            var op = await mainRepository.AddStates(resultDto);


            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadCity()
        {
            var result = new List<CityCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\City3.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<CityCsv>().Distinct().ToList();
            }
            var resultDto = result.GroupBy(ss => ss.City)
            .Select(ss => new CityDto { Name = ss.Key, StateName = ss.First().State })
            .ToList();
            //result.Select(s => s.City).Distinct().ToList();
            var op = await mainRepository.AddCities(resultDto);


            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadProduct()
        {
            ReadTranslateCategoryName();
            var result = new List<ProductCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\Product.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<ProductCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new ProductDto
            {
                Id = s.Id,
                Width = s.Width,
                Weight = s.Weight,
                Length = s.Length,
                CategoryName = TranslateCategoryName(s.Name),
                Height = s.Height
            }).ToList();
            var op = await mainRepository.AddProducts(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadSeller()
        {
            var result = new List<SellerCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\sellers2.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<SellerCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new SellerDto
            {
                Id = s.Id,
                ZipCodePrefix = s.ZipCodePrefix,
                City = s.City,
                State = s.State
            }).ToList();
            var op = await mainRepository.AddSellers(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadCustomer()
        {
            var result = new List<CustomerCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\Customer.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<CustomerCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new CustomerDto
            {
                Id = s.Id,
                CityName = s.CityName
            }).ToList();
            var op = await mainRepository.AddCustomers(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadOrder()
        {
            var result = new List<OrderCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\Orders.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<OrderCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new OrderDto
            {
                Id = s.Id,
                CustomerId = s.CustomerId,
                PurchaseDate = s.PurchaseDate
            }).ToList();
            var op = await mainRepository.AddOrders(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadOrderItem()
        {
            var result = new List<OrderItemCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\OrderItems.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<OrderItemCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new OrderItemDto
            {
                FreightValue = s.FreightValue,
                OrderId = s.OrderId,
                Price = s.Price,
                ProductId = s.ProductId,
                SellerId = s.SellerId
            }).ToList();
            var op = await mainRepository.AddOrderItems(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> ReadGeoLocation()
        {
            var result = new List<GeoLocationCsv>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\GeoLocations.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<GeoLocationCsv>().Distinct().ToList();
            }
            var resultDto = result.Select(s => new GeoLocationDto
            {
                CityName = s.CityName,
                Lat = s.Lat,
                Lng = s.Lng,
                ZipCodePrefix = s.ZipCodePrefix
            }).ToList();
            var op = await mainRepository.AddGeoLocations(resultDto);
            return new JsonResult(op) { StatusCode = 200 };
        }


        List<TranslateCategoryCsv> CategoryCsvs = new List<TranslateCategoryCsv>();

        
        
        private void ReadTranslateCategoryName()
        {
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\TranslateCategoryName.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                CategoryCsvs = csv.GetRecords<TranslateCategoryCsv>().Distinct().ToList();
            }
        }

        private string TranslateCategoryName(string categoryName)
        {
            var result = CategoryCsvs.FirstOrDefault(c=> c.CategoryName == categoryName);
            return result == null ? "other" : result.CategoryNameEnglish;
        }




    }
}
