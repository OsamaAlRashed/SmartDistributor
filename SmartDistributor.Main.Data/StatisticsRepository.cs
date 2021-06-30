using Microsoft.EntityFrameworkCore;
using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Idata;
using SmartDistributor.SharedKernel;
using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Data
{
    public class StatisticsRepository : BaseRepository , IStatisticsRepository
    {
        public StatisticsRepository(SmartDistributorDbContext context):base(context)
        {

        }

        public Task<bool> EditCategoryForProduct(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ProductDto>> GetBestProductInCategory()
        {
            //var result = Context.Products
            //                    .GroupBy(p => p.CategoryId)
            //                    .Select(p => new ProductDto
            //                    {
            //                        CategoryName = p.First().Category.Name,
            //                        Id = p.Where()
            //                    })
            return default;
        }

        public async Task<IEnumerable<ProductDto>> GetBestProducts()
        {
            try
            {
                return await Context.Products
                    .OrderByDescending(p => p.OrderItems.Count())
                    .Select(p => new ProductDto
                    {
                        CategoryName = p.Category.Name,
                        Weight = p.Weight,
                        OrderItemsCount = p.OrderItems.Count()
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<object> GetCategoryCountBySeller()
        {
            var result = Context.OrderItems.Include(p => p.Product)
                         .ToList()
                         .GroupBy(o => new { o.Product.CategoryId , o.SellerId })
                         .Select(og => new
                         {
                             ProductId = og.Key.CategoryId,
                             SellerId = og.Key.SellerId,
                             Count = og.Count()
                         })
                         .ToList();
            return result;
        }

        public async Task<CountDto> GetCounts()
        {
            try
            {
                return new CountDto
                {
                    CategoryCount = await Context.Categories.CountAsync(),
                    CityCount = await Context.Cities.CountAsync(),
                    StateCount = await Context.States.CountAsync(),
                    CustomerCount = await Context.Customers.CountAsync(),
                    GeoLocationCount = await Context.Geolocations.CountAsync(),
                    OrderCount = await Context.Orders.CountAsync(),
                    OrderItemCount = await Context.OrderItems.CountAsync(),
                    ProductCount = await Context.Products.CountAsync(),
                    SellerCount = await Context.Sellers.CountAsync(),
                };
            }
            catch (Exception ex)
            {
                return new CountDto();
            }
            
        }

        public async Task<object> GetProAndCatInState()
        {
            var orderItems = Context.OrderItems
                                    .Include(o => o.Seller)
                                    .ToList()
                                    .GroupBy(o => o.Seller.State)
                                    .Select(o => new {
                                        State = o.Key,
                                        Count = o.Count()
                                    })
                                    .ToList();
            return orderItems;
        }

        public async Task<List<SellerProductsCountDto>> GetProductCountForSellers()
        {

            var orderItemsList = Context.OrderItems
                         .ToList()
                         .GroupBy(o => o.SellerId)
                         .Select(og => new SellerProductsCountDto
                         {
                             SellerId = og.Key,
                             Count = og.Count()
                         })
                         .ToList();
            return orderItemsList;
        }

        public async Task<int> GetProductsCountBySeller(int zipCodePrefix)
        {
            return Context.OrderItems.Include(x => x.Seller).Count(x => x.Seller.ZipCodePrefix == zipCodePrefix);
        }

        public async Task<IEnumerable<object>> GetProductsCountInCategories()
        {
            var result = await Context.Categories
                                .Select(c => new CategoryDto
                                {
                                    Name = c.Name,
                                    Id = c.Id,
                                    ProductsCount = c.Products.Count()
                                }).ToListAsync();

            return result;
            

        }

        public async Task<string> GetProductsSellersCount()
        {
            List<char> MyEnum = new List<char>();
            StringBuilder sb = new StringBuilder();
            
            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();

            var orderItemsList = Context.OrderItems
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             ProductId = og.Key.ProductId,
                             SellerId = og.Key.SellerId,
                             Count = og.Count()})
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count;
            }

            var Sellers = await Context.Sellers.ToListAsync();
            var Products = await Context.Products.ToListAsync();
            foreach (var product in Products)
            {
                for (int i = 0; i < Sellers.Count; i++)
                {
                    if(!orderItems.ContainsKey((product.Id , Sellers[i].Id)))
                    {
                        sb.Append('0');
                    }
                    else
                    {
                        sb.Append((orderItems[(product.Id, Sellers[i].Id)]).ToString());
                    }
                    if(i != (Products.Count - 1))
                    {
                        sb.Append(',');
                    }
                }
                sb.AppendLine();
            }
            string x = sb.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\cxc.csv", x);
            return "";
            
        }

        public async Task<IEnumerable<object>> GetTop2000Products()
        {
            try
            {
                int AllProducts = Context.Products.Count();
                var rates = Context.Categories.Select(c => new { Count = (int)(c.Products.Count() * 1.0 / AllProducts * 2000), Id = c.Id }).ToList();
                Dictionary<Guid, int> dicRates = new Dictionary<Guid, int>();
                foreach (var item in rates)
                {
                    dicRates[item.Id] = item.Count;
                }
                var result = Context.Categories
                                    .Include(c =>c.Products)
                                    .ThenInclude(p => p.OrderItems)
                                    .AsEnumerable()
                                    .Select(c => new
                                    {
                                        Category = c.Name,
                                        Count = dicRates[c.Id],
                                        TopProducts = c.Products
                                                       .OrderByDescending(p => p.OrderItems.Count())
                                                       .Select(p => new //ProductDto
                                                       {
                                                           Id = p.Id,
                                                           //Width = p.Width,
                                                           //Length = p.Length,
                                                           //Height = p.Height,
                                                           //Weight = p.Weight,
                                                           OrderItemsCount = p.OrderItems.Count(),
                                                       }).Take(dicRates[c.Id]).ToList(),
                                    }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<int>> GetSellerwithoutGeo()
        {
            List<int> sellers = Context.Sellers.Select(x => x.ZipCodePrefix).ToList();
            List<int> geos = Context.Geolocations.Select(x => x.ZipCodePrefix).ToList();
            List<int> x = new List<int>();
            int i = 0;
            foreach (var s in sellers)
            {
                if (!geos.Contains(s))
                {
                    x.Add(s);
                }
            }
            return x;

        }

        public async Task<bool> SetSellerZipPrefex(int pa , int newpa)
        {
            var sellers = Context.Sellers.Where(x => x.ZipCodePrefix == pa).ToList();

            foreach (var seller in sellers)
            {
                seller.ZipCodePrefix = newpa;
            }
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
