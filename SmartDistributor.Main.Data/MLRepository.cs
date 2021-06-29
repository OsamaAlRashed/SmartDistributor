using Microsoft.EntityFrameworkCore;
using SmartDistributor.Main.Dto;
using SmartDistributor.Main.Idata;
using SmartDistributor.Models.Main;
using SmartDistributor.SqlServer.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartDistributor.Main.Data
{
    public class MLRepository : BaseRepository , IMLRepository
    {
        public MLRepository(SmartDistributorDbContext context):base(context)
        {

        }

        public async Task<bool> EditSellersClusters(List<int> sellerclusters)
        {
            try
            {
                int i = 0;
                var sellers = Context.Sellers.ToList();
                foreach (var seller in sellers)
                {
                    seller.ClusterId = sellerclusters[i];
                    i++;
                }
                await Context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }

        public async Task<string> GenerateFullSellerProductCsv()
        {
            StringBuilder s = new StringBuilder();

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = Context.OrderItems.Where(o => !o.Product.Category.IsIgnored).Include(o => o.Seller)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count > 0 ? 1 : 0;
            }

            var products = Context.Products.Where(o => !o.Category.IsIgnored).Include(cat => cat.Category).ToList();
            var sellers = Context.Sellers.ToList();

            foreach (var product in products)
            {
                s.Append((product.Category.Number).ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');
                for (int i = 0; i < sellers.Count ; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, sellers[i].Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append('1');
                    }
                    if (i != sellers.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\ProductFullSeller.csv", x);
            return "";
        }

        public async Task<string> GenerateSellerCategoryCsv()
        {
            StringBuilder s = new StringBuilder();

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = Context.OrderItems.Include(o => o.Product)
                         .Where(o => !o.Product.Category.IsIgnored)
                         .ToList()  
                         .GroupBy(o => new { o.Product.CategoryId, o.SellerId })
                         .Select(og => new
                         {
                             CategoryId = og.Key.CategoryId,
                             SellerId = og.Key.SellerId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.CategoryId, item.SellerId)] = item.Count;
            }

            var sellers = Context.Sellers.ToList();
            var categories = Context.Categories.Where(o => !o.IsIgnored).ToList();
            int count = 0;
            foreach (var seller in sellers)
            {
                for (int i = 0; i < categories.Count ; i++)
                {
                    if (!orderItems.ContainsKey((categories[i].Id, seller.Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append((orderItems[(categories[i].Id, seller.Id)]).ToString());
                    }
                    if (i != (categories.Count - 1))
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\sellerCategory.csv", x);
            return "";
        }

        public async Task<string> GenerateSellerClusterCsv()
        {
            StringBuilder s = new StringBuilder();

            Dictionary<ValueTuple<Guid, int>, int> orderItems = new Dictionary<(Guid, int), int>();
            var orderItemsList = Context.OrderItems.Include(o => o.Seller)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.Seller.ClusterId })
                         .Select(og => new
                         {
                             ClusterId = og.Key.ClusterId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.ClusterId)] = item.Count > 0 ? 1 : 0;
            }

            int ClusterNum = 64;
            var products = Context.Products.Where(s => !s.Category.IsIgnored).Include(p => p.Category).ToList();

            foreach (var product in products)
            {

                s.Append(product.Category.Number.ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');
                for (int i = 0; i < ClusterNum; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, i)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append((orderItems[(product.Id, i)]).ToString());
                    }
                    if (i != ClusterNum)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();

            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\ProductSellerCluster.csv", x);
            return "";
        }

        public async Task<string> GenerateSellerCsv(List<int> ids)
        {
            StringBuilder s = new StringBuilder();
            List<Guid> geoIds = new List<Guid>();
            List<int> sellersWithoutGeo = new List<int>();
            var geoList = await Context.Geolocations
                                  .Include(g => g.City)
                                  .ThenInclude(g => g.State)
                                  .ToListAsync();
            foreach (var id in ids)
            {
                var geo = geoList.Where(g => g.ZipCodePrefix == id && !geoIds.Contains(g.Id))
                                  .FirstOrDefault();

                if(geo == null)
                {
                    sellersWithoutGeo.Add(id);
                }
                else
                {
                    geoIds.Add(geo.Id);
                    s.Append(geo.Lat + "," + geo.Lng + "," + geo.City.Name + "," + geo.City.State.Name);
                    s.AppendLine();
                }
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\seller.csv", x);
            return "";
        }

        public async Task<bool> FillIsChooseProduct(Guid catId)
        {
            await Context.Products.Where(c => c.CategoryId == catId)
                .ForEachAsync(
                    x => x.IsChoose = false
                );
            await Context.SaveChangesAsync();
            return true;             
        }

        public async Task<string> GenerateFullSellerProductV2()
        {
            StringBuilder s = new StringBuilder();

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = Context.OrderItems
                         .Include(o => o.Seller)
                         .Include(o => o.Product)
                         //.Where(x => x.Product.IsChoose)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count;// > 0 ? 1 : 0;
            }
            //.Where(x => x.IsChoose)
            var products = Context.Products.Include(cat => cat.Category).ToList();
            var sellers = Context.Sellers.ToList();

            foreach (var product in products)
            {
                s.Append((product.Category.Number).ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');
                for (int i = 0; i < sellers.Count; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, sellers[i].Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append(orderItems[(product.Id, sellers[i].Id)].ToString());
                    }
                    if (i != sellers.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\ProductFullSellerKmeans.csv", x);
            return "";
        }

        public async Task<bool> Choose2000Product()
        {
            await Context.Products
                .ForEachAsync(
                    x => x.IsChoose = false
                );
            await Context.SaveChangesAsync();


            int AllProducts = Context.Products.Where(p => !p.Category.IsIgnored).Count();
            var rates = Context.Categories.Where(p => !p.IsIgnored).Select(c => new { Count = (int)(c.Products.Count() * 1.0 / AllProducts * 2000), Id = c.Id }).ToList();
            Dictionary<Guid, int> dicRates = new Dictionary<Guid, int>();
            foreach (var item in rates)
            {
                dicRates[item.Id] = item.Count;
            }
            var result = Context.Categories
                                .Include(c => c.Products)
                                .ThenInclude(p => p.OrderItems)
                                .AsEnumerable()
                                .SelectMany(c => 
                                
                                    //Category = c.Name,
                                    //Count = dicRates[c.Id],
                                   c.Products
                                                   .OrderByDescending(p => p.OrderItems.Count())
                                                   .Select(p => new
                                                       {
                                                           Id = p.Id,
                                                           OrderItemsCount = p.OrderItems.Count(),
                                                   }).Take(dicRates[c.Id]).ToList()
                                ).ToList();

            //foreach (var item in result)
            //{
            //    Context.Products.Where()item.Id
            //}


            return true;
        }

        public async Task<bool> GenerateNewProductCsv()
        {
            StringBuilder s = new StringBuilder();

            //Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            //var orderItemsList = Context.OrderItems
            //             .Include(o => o.Seller)
            //             .Include(o => o.Product)
            //             .ToList()
            //             .GroupBy(o => new { o.ProductId, o.SellerId })
            //             .Select(og => new
            //             {
            //                 SellerId = og.Key.SellerId,
            //                 ProductId = og.Key.ProductId,
            //                 Count = og.Count()
            //             })
            //             .ToList();


            //var productWithDate = Context.OrderItems
            //    .Include(x => x.Order)
            //               .Where(x => x.Product.IsChoose)
            //               .ToList()
            //               .GroupBy(x => x.ProductId)
            //               .Select(x => new
            //               {
            //                   ProductId = x.Key,
            //                   MaxMonth = x.Max(o => o.Order.PurchaseDate.Month)
            //               }).ToList();


            //Dictionary<Guid , int> proDate = new Dictionary<Guid , int>();

            //foreach (var item in productWithDate)
            //{
            //    proDate[item.ProductId] = item.MaxMonth;
            //}

            //foreach (var item in orderItemsList)
            //{
            //    orderItems[(item.ProductId, item.SellerId)] = item.Count > 0 ? 1 : 0;
            //}

            var products = Context.Products.Include(cat => cat.Category).ToList();
            //var sellers = Context.Sellers.ToList();

            foreach (var product in products)
            {
                s.Append((product.Category.Number).ToString() + ','
                    //   + proDate[product.Id].ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');
                //for (int i = 0; i < sellers.Count; i++)
                //{
                //    if (!orderItems.ContainsKey((product.Id, sellers[i].Id)))
                //    {
                //        s.Append('0');
                //    }
                //    else
                //    {
                //        s.Append('1');
                //    }
                //    if (i != sellers.Count)
                //    {
                //        s.Append(',');
                //    }
                //}
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\NewProductsDate.csv", x);
            return true;


        }

        public async Task<bool> GenerateTop2000ProductsCsv()
        {
            await Context.Products
                .ForEachAsync(
                    x => x.IsChoose = false
                );
            await Context.SaveChangesAsync();

            StringBuilder s = new StringBuilder();

            int AllProducts = Context.Products.Count();
            var rates = Context.Categories.Select(c => new { Count = (int)(c.Products.Count() * 1.0 / AllProducts * 2000), Id = c.Id }).ToList();
            Dictionary<Guid, int> dicRates = new Dictionary<Guid, int>();
            foreach (var item in rates)
            {
                dicRates[item.Id] = item.Count;
            }

            var result = Context.Categories
                                .Include(c => c.Products)
                                .ThenInclude(p => p.OrderItems)
                                .Where(c => !c.IsIgnored)
                                .AsEnumerable()
                                .SelectMany(c => c.Products
                                              .OrderByDescending(p => p.OrderItems.Count())
                                              .Select(p =>  p.Id
                                              ).Take(dicRates[c.Id]).ToList()
                                ).ToList();

            await Context.Products.Where(p => result.Contains(p.Id)).ForEachAsync(
                    p => p.IsChoose = true
            );

            await Context.SaveChangesAsync();

            var chooseProducts = Context.Products.Where(p => p.IsChoose).ToList();

            var query = Context.OrderItems;

            var productWithDate = query
                .Include(x => x.Order)
                           .Where(x => x.Product.IsChoose)
                           .ToList()
                           .GroupBy(x => x.ProductId)
                           .Select(x => new
                           {
                               ProductId = x.Key,
                               MaxMonth = x.Max(o => o.Order.PurchaseDate.Month),
                               // CategoryProducts = catsDic[Context.Categories.SingleOrDefault(s => s.Products.Select(x =>x.Id).Contains(x.Key)).Id]
                           }).ToList();


            Dictionary<Guid, int> proDate = new Dictionary<Guid, int>();

            foreach (var item in productWithDate)
            {
                proDate[item.ProductId] = item.MaxMonth;
            }

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = query.Where(o => o.Product.IsChoose).Include(o => o.Seller)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count > 0 ? 1 : 0;
            }


            var sellersChoose = query.Include(o => o.Seller).Where(s => s.Product.IsChoose).Select(o => o.Seller).Distinct().ToList();


            var cats = query
                         .Include(o => o.Seller)
                         .Include(o => o.Product)
                         .Where(x => x.Product.IsChoose)
                         .ToList()
                         .GroupBy(o => new { o.Product.CategoryId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             CategoryId = og.Key.CategoryId,
                             Count = og.Count()
                         })
                         .ToList();

            Dictionary<ValueTuple<Guid, Guid>, int> catsDic = new Dictionary<(Guid, Guid), int>();

            foreach (var item in cats)
            {
                catsDic[(item.CategoryId, item.SellerId)] = item.Count;
            }

            foreach (var product in chooseProducts)
            {
                s.Append((product.Category.Number).ToString() + ','
                       + proDate[product.Id].ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');

                for (int i = 0; i < sellersChoose.Count; i++)
                {
                    if (!catsDic.ContainsKey((product.CategoryId, sellersChoose[i].Id)))
                    {
                        s.Append("0,");
                    }
                    else
                    {
                        s.Append(catsDic[(product.CategoryId, sellersChoose[i].Id)] + ",");
                    }

                }

                for (int i = 0; i < sellersChoose.Count; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, sellersChoose[i].Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append('1');
                    }
                    if (i != sellersChoose.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\Products2000.csv", x);
            return true;
        }

        public async Task<bool> GenerateProductsV3()
        {
            StringBuilder s = new StringBuilder();

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = Context.OrderItems
                         .Include(o => o.Seller)
                         .Include(o => o.Product)
                         .Where(x => x.Product.IsChoose)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            var cats = Context.OrderItems
                         .Include(o => o.Seller)
                         .Include(o => o.Product)
                         .Where(x => x.Product.IsChoose)
                         .ToList()
                         .GroupBy(o => new { o.Product.CategoryId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             CategoryId = og.Key.CategoryId,
                             Count = og.Count()
                         })
                         .ToList();

            Dictionary<ValueTuple<Guid, Guid>, int> catsDic = new Dictionary<(Guid, Guid), int>();

            foreach (var item in cats)
            {
                catsDic[(item.CategoryId ,item.SellerId)] = item.Count ;
            }


            var productWithDate = Context.OrderItems
                .Include(x => x.Order)
                           .Where(x => x.Product.IsChoose)
                           .ToList()
                           .GroupBy(x => x.ProductId)
                           .Select(x => new
                           {
                               ProductId = x.Key,
                               MaxMonth = x.Max(o => o.Order.PurchaseDate.Month),
                              // CategoryProducts = catsDic[Context.Categories.SingleOrDefault(s => s.Products.Select(x =>x.Id).Contains(x.Key)).Id]
                           }).ToList();


            Dictionary<Guid, int> proDate = new Dictionary<Guid, int>();

            foreach (var item in productWithDate)
            {
                proDate[item.ProductId] = item.MaxMonth;
            }

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count > 0 ? 1 : 0;
            }

            var products = Context.Products.Where(x => x.IsChoose).Include(cat => cat.Category).ToList();
            var sellers = Context.Sellers.ToList();

            foreach (var product in products)
            {
                s.Append((product.Category.Number).ToString()  + ','
                       + proDate[product.Id].ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ',');

                for (int i = 0; i < sellers.Count; i++)
                {
                    if (!catsDic.ContainsKey((product.CategoryId , sellers[i].Id)))
                    {
                        s.Append("0,");
                    }
                    else
                    {
                        s.Append(catsDic[(product.CategoryId, sellers[i].Id)] + ",");
                    }
                    
                }


                for (int i = 0; i < sellers.Count; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, sellers[i].Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append('1');
                    }
                    if (i != sellers.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\NewProductsV3.csv", x);
            return true;

        }

        public async Task<bool> GenerateProducts2000V4()
        {
            await Context.Products
                .ForEachAsync(
                    x => x.IsChoose = false
                );
            await Context.SaveChangesAsync();

            StringBuilder s = new StringBuilder();

            int AllProducts = Context.Products.Count();
            var rates = Context.Categories.Where(c => !c.IsIgnored).Select(c => new { Count = (int)(c.Products.Count() * 1.0 / AllProducts * 2000), Id = c.Id }).ToList();
            Dictionary<Guid, int> dicRates = new Dictionary<Guid, int>();
            foreach (var item in rates)
            {
                dicRates[item.Id] = item.Count;
            }

            var result = Context.Categories
                                .Include(c => c.Products)
                                .ThenInclude(p => p.OrderItems)
                                .Where(c => !c.IsIgnored)
                                .AsEnumerable()
                                .SelectMany(c => c.Products
                                              .OrderByDescending(p => p.OrderItems.Count())
                                              .Select(p => p.Id
                                              ).Take(dicRates[c.Id]).ToList()
                                ).ToList();

            await Context.Products.Where(p => result.Contains(p.Id)).ForEachAsync(
                    p => p.IsChoose = true
            );

            await Context.SaveChangesAsync();


            var chooseProducts = Context.Products.Where(p => p.IsChoose).ToList();

            Dictionary<ValueTuple<Guid, Guid>, int> orderItems = new Dictionary<(Guid, Guid), int>();
            var orderItemsList = Context.OrderItems.Where(o => o.Product.IsChoose).Include(o => o.Seller)
                         .ToList()
                         .GroupBy(o => new { o.ProductId, o.SellerId })
                         .Select(og => new
                         {
                             SellerId = og.Key.SellerId,
                             ProductId = og.Key.ProductId,
                             Count = og.Count()
                         })
                         .ToList();

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.SellerId)] = item.Count > 0 ? 1 : 0;
            }


            var sellersChoose = Context.OrderItems.Include(o => o.Seller).Where(s => s.Product.IsChoose).Select(o => o.Seller).Distinct().ToList();

            foreach (var product in chooseProducts)
            {
                for (int i = 1; i <= chooseProducts.Count; i++)
                {
                    for(int j = 1; j < i; j++)
                    {
                        s.Append("0,");
                    }
                    s.Append("1,");
                    for (int k = i; k <= chooseProducts.Count; k++)
                    {
                        s.Append("0,");
                    }
                }


                for (int i = 0; i < sellersChoose.Count; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, sellersChoose[i].Id)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append('1');
                    }
                    if (i != sellersChoose.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\Products2000V4.csv", x);
            return true;

        }

        public Task<bool> GenerateAllProductsV4()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GenerateFileCsvV5()
        {

            StringBuilder s = new StringBuilder();
            Dictionary<ValueTuple<Guid, string>, int> orderItems = new Dictionary<(Guid, string), int>();
            
            var orderItemsList = Context.OrderItems
                         .Include(o => o.Seller)
                         .Include(p => p.Product)
                         .ToList()
                         .GroupBy(x => new { x.ProductId, x.Seller.State })
                         .Select(y => new
                         {
                             ProductId = y.Key.ProductId,
                             State = y.Key.State,
                             Count = y.Count()
                         })
                        .ToList();

            var productsCount = Context.Products
                                       .Include(p => p.OrderItems)
                                       .Select(p => new { 
                                           ProductId = p.Id,
                                           Count = p.OrderItems.Count()
                                       })
                                       .ToList();

            Dictionary<Guid, int> productsCountDic = new Dictionary<Guid, int>();

            foreach (var item in productsCount)
            {
                productsCountDic[item.ProductId] = item.Count;
            }

            foreach (var item in orderItemsList)
            {
                orderItems[(item.ProductId, item.State)] = item.Count;
            }

            var chooseState = orderItemsList.Select(o => o.State).ToList();

            var products = Context.Products.Include(cat => cat.Category).ToList();
            var states = Context.States.Where(s => chooseState.Contains(s.Name)).ToList();

            foreach (var product in products)
            {
                s.Append((product.Category.Number).ToString() + ','
                       + product.Height + ','
                       + product.Weight + ','
                       + product.Width + ','
                       + product.Length + ','
                       + productsCountDic[product.Id] + ',');
                for (int i = 0; i < states.Count; i++)
                {
                    if (!orderItems.ContainsKey((product.Id, states[i].Name)))
                    {
                        s.Append('0');
                    }
                    else
                    {
                        s.Append('1');
                    }
                    if (i != states.Count)
                    {
                        s.Append(',');
                    }
                }
                s.AppendLine();
            }

            string x = s.ToString();
            File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\FileV5.csv", x);
            return true;


        }

        public async Task<bool> FillCategoriesNumber()
        {
            int i = 0;
            await Context.Categories.ForEachAsync(
                    x => x.Number = i++
                );
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GenerateFinalCsv()
        {
            List<List<Product>> fullProduct = new List<List<Product>>();

            return true;
        }

        public async Task<bool> RemoveGeoLocationAndCity()
        {
            //List<int> sellers = Context.Sellers.Select(x => x.ZipCodePrefix).ToList();

            //Dictionary<int, int> sellersDic = new Dictionary<int, int>();

            //foreach (var item in sellers)
            //{
            //    sellersDic[item] = 1;
            //}


            //var geos = Context.Geolocations.Where(geo => !sellers.Contains(geo.ZipCodePrefix)).ToList();




            List<int> sellers = Context.Sellers.Select(x => x.ZipCodePrefix).ToList();
            //var cities = Context.Cities.Where(city => !city.Geolocations.Any()).ToList();
            var cities = Context.Cities
                                .Where(city => city.Geolocations
                                                    .All(g => !sellers.Contains(g.ZipCodePrefix))).ToList();

            for (int i = 0; i < 10; i++)
            {
                cities.Skip(1000 * i).Take(1000);
                Context.Cities.RemoveRange(cities);
                await Context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> FilterGeoLocationAndCity()
        {
            List<int> sellers = Context.Sellers.Select(x => x.ZipCodePrefix).ToList();
            return default;


        }

        public async Task<bool> AddGeoLocation(GeoLocationDto geoLocationDto)
        {
            Context.Geolocations.Add(new Geolocation
            {
                Id = Guid.NewGuid(),
                Lat = geoLocationDto.Lat,
                Lng = geoLocationDto.Lng,
                CityId = geoLocationDto.CityId,
                ZipCodePrefix = geoLocationDto.ZipCodePrefix,
            });
            await Context.SaveChangesAsync();
            return true;
        }
    }
}
