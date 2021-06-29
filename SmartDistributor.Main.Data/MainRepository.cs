using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Idata;
using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using SmartDistributor.Models.Main;
using Microsoft.EntityFrameworkCore;
using SmartDistributor.SharedKernel;

namespace SmartDistributor.Main.Data
{
    public class MainRepository : BaseRepository ,  IMainRepository
    {
        public MainRepository(SmartDistributorDbContext context):base(context)
        {

        }

        public async Task<bool> AddStates(List<string> states)
        {
            try
            {
                var statesSet = states.Select(s => new State
                {
                    Id = Guid.NewGuid(),
                    Name = s,
                }).Distinct().ToList();
                Context.States.AddRange(statesSet);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddCategory(List<string> categorys)
        {
            try
            {
                var categories = categorys.Select(s => new Category
                {
                    Id = Guid.NewGuid(),
                    Name = s,
                }).Distinct().ToList();
                Context.Categories.AddRange(categories);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddCities(List<CityDto> cityDtos)
        {
            try
            {
                var oldCities = Context.Cities.ToList().Select(c => c.Name);
                var cities = cityDtos.Where(c => !oldCities.Contains(c.Name)).Select(c => new City
                {
                    Id = Guid.NewGuid(),
                    Name = c.Name,
                    StateId = Context.States.FirstOrDefault(s => s.Name == c.StateName).Id
                }).Distinct().ToList();

                Context.Cities.AddRange(cities);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddProducts(List<ProductDto> productDtos)
        {
            try
            {
                var products = productDtos.Select(s => new Product
                {
                    Id = s.Id,
                    CategoryId = GetCategoryByName(s.CategoryName),
                    Height = s.Height,
                    Length = s.Length,
                    Weight = s.Weight,
                    Width = s.Width,
                }).Distinct().ToList();

                Context.Products.AddRange(products);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private Guid GetCategoryByName(string Name)
        {
            var result = Context.Categories.FirstOrDefault(c => c.Name == Name);
            if (result == null)
            {
                return Context.Categories.FirstOrDefault(c => c.Name == "other").Id;
            }
            return result.Id;
        }

        


        public async Task<bool> AddSellers(List<SellerDto> sellerDtos)
        {
            try
            {
                var sellers = sellerDtos.Select(s => new Seller
                {
                    Id = s.Id,
                    ZipCodePrefix = s.ZipCodePrefix,
                    City = s.City,
                    State = s.State
                }).Distinct().ToList();

                Context.Sellers.AddRange(sellers);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddCustomers(List<CustomerDto> customerDtos)
        {
            try
            {
                var customers = customerDtos.Select(s => new Customer
                {
                    Id = s.Id,
                    CityId = GetCityIdByName(s.CityName),
                }).Distinct().ToList();

                Context.Customers.AddRange(customers);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private Guid GetCityIdByName(string Name)
        {
            if (c.ContainsKey(Name))
            {
                return c[Name];
            }
            return c[""];
        }
        Dictionary<string, Guid> c = new Dictionary<string, Guid>();
        public async Task<bool> AddGeoLocations(List<GeoLocationDto> geoLocationDtos)
        {
            try
            {
                var cities = await Context.Cities.ToListAsync();
                foreach (var city in cities)
                {
                    c[city.Name] = city.Id;
                }

                List<int> sellers = Context.Sellers.Select(x => x.ZipCodePrefix).ToList();

                Dictionary<int, int> sellersDic = new Dictionary<int, int>();
                //.Where(geo => sellers.Contains(geo.ZipCodePrefix))
                var geoLocations = geoLocationDtos
                .Select(s => new Geolocation
                {
                    Id = Guid.NewGuid(),
                    Lng = s.Lng,
                    ZipCodePrefix = s.ZipCodePrefix,
                    Lat = s.Lat,
                    CityId = GetCityIdByName(s.CityName),
                }).DistinctBy(x => x.ZipCodePrefix).ToList();


                for (int i = 0; i < 10; i++)
                {
                    await Context.Geolocations.AddRangeAsync(geoLocations.Skip(27000 * i).Take(27000));
                    await Context.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddOrders(List<OrderDto> orderDtos)
        {
            try
            {
                var orders = orderDtos.Select(o => new Order
                {
                    CustomerId = o.CustomerId,
                    Id = o.Id,
                    PurchaseDate = o.PurchaseDate
                }).ToList();

                Context.Orders.AddRange(orders);
                await Context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> AddOrderItems(List<OrderItemDto> orderItemDtos)
        {
            try
            {
                var newOrderItems = FilterInvalidProduct(orderItemDtos);
                var orderItems = newOrderItems.Select(o => new OrderItem
                {
                    FreightValue = o.FreightValue,
                    OrderId = o.OrderId,
                    ProductId = o.ProductId,
                    SellerId = o.SellerId,
                    Price = o.Price,
                    Id = Guid.NewGuid()
                }).ToList();

                Context.OrderItems.AddRange(orderItems);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        private List<OrderItemDto> FilterInvalidProduct(List<OrderItemDto> orderItemDtos)
        {
            var productIds = Context.Products.Select(p => p.Id).ToList();
            List<OrderItemDto> neworderItems = orderItemDtos.Where(o => productIds.Contains(o.ProductId)).ToList();
            return neworderItems;
        }

    }
}
