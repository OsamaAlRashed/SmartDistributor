using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using System.Collections.Generic;
using System.Linq;
using SmartDistributor.Main.Idata;
using System;
using SmartDistributor.Api.Models;
using SmartDistributor.Main.Dto;

namespace SmartDistributor.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MLController : ControllerBase
    {
        private readonly IMLRepository mLRepository;

        public MLController(IMLRepository mLRepository)
        {
            this.mLRepository = mLRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateProSellerClass()
        {
            var sb = new StringBuilder();
            for (int i = 1; i < 75; i++)
            {
                sb.Append($"[Name(\"Cat{i}\")]");
                sb.AppendLine();
                sb.Append($"public int Cat{i}");
                sb.Append(@" { get; set; }");
                sb.AppendLine();
            }
            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\class.txt", text);
            return new JsonResult(true) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateProSellerColumn()
        {
            var sb = new StringBuilder();
            for (int i = 1; i <= 75; i++)
            {
                //sb.Append($"Cat{i} = p.Cat{i},");

                sb.Append($"Cat{i},");

                //sb.Append($"\"Cat{i}\" ,");
                //if(i % 5 == 0)
                //{
                //    sb.AppendLine();
                //}
                //sb.AppendLine();
            }

            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\class.txt", text);
            return new JsonResult(true) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateSellerCsv()
        {
            var result = new List<int>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\sellerZip.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<int>().ToList();
            }

            var op = await mLRepository.GenerateSellerCsv(result);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateSellerCategoryCsv()
        {
            var op = await mLRepository.GenerateSellerCategoryCsv();
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> EditSellersClusters()
        {
            var result = new List<int>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\SellerCluster.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                //csv.Configuration.RegisterClassMap<>();
                result = csv.GetRecords<int>().ToList();
            }

            var op = await mLRepository.EditSellersClusters(result);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateSellerClusterCsv()
        {
            var op = await mLRepository.GenerateSellerClusterCsv();
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFullSellerProductCsv()
        {
            var op = await mLRepository.GenerateFullSellerProductCsv();
            return new JsonResult(op) { StatusCode = 200 };
        }


        [HttpGet]
        public async Task<IActionResult> FillIsChooseProduct(Guid guid)
        {

            var op = await mLRepository.FillIsChooseProduct(guid);
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFullSellerProductV2()
        {

            var op = await mLRepository.GenerateFullSellerProductV2();
            return new JsonResult(op) { StatusCode = 200 };
        }


        [HttpGet]
        public async Task<IActionResult> GenerateNewProductCsv()
        {
            var op = await mLRepository.GenerateNewProductCsv();
            return new JsonResult(op) { StatusCode = 200 };
        }


        [HttpGet]
        public async Task<IActionResult> GenerateProductsV3()
        {
            var op = await mLRepository.GenerateProductsV3();
            return new JsonResult(op) { StatusCode = 200 };
        }


        [HttpGet]
        public async Task<IActionResult> GenerateTop2000ProductsCsv()
        {
            var op = await mLRepository.GenerateTop2000ProductsCsv();
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateProducts2000V4()
        {
            var op = await mLRepository.GenerateProducts2000V4();
            return new JsonResult(op) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> EncodingProducts()
        {
            var s = new StringBuilder();
            for (int i = 1; i <= 1925; i++)
            {
                for (int j = 1; j < i; j++)
                {
                    s.Append("0,");
                }
                s.Append("1");
                if (i != 1925)
                {
                    s.Append(",");
                }
                for (int k = i; k <= 1925; k++)
                {
                    s.Append("0");
                    if (i != 1925)
                    {
                        s.Append(",");
                    }
                }

                s.AppendLine();
            }


            var text = s.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\xx.txt", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFileCsvV5()
        {
            var result = mLRepository.GenerateFileCsvV5();

            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRandom()
        {
            Random r = new Random();


            var s = new StringBuilder();

            for (int i = 1; i <= 32950; i++)
            {
                int randCount = r.Next(1, 4);
                int p1 = 0, p2 = 0, p3 = 0;
                if (randCount == 1)
                {
                    p1 = r.Next(1, 17);
                }
                else if (randCount == 2)
                {
                    p1 = r.Next(1, 8);
                    p2 = r.Next(8, 17);
                }
                else
                {
                    p1 = r.Next(1, 5);
                    p2 = r.Next(5, 10);
                    p3 = r.Next(10, 16);
                }
                for (int j = 1; j <= 16; j++)
                {
                    if (j == p1)
                    {
                        s.Append("1,");
                    }
                    else if (j == p2 && randCount >= 2)
                    {
                        s.Append("1,");
                    }
                    else if (j == p3 && randCount >= 3)
                    {
                        s.Append("1,");
                    }
                    else
                    {
                        s.Append("0,");
                    }
                }
                s.AppendLine();
            }


            var text = s.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\xx.txt", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> AggregateSellers()
        {
            int[,] A = new int[3095, 64];
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\sellerCategory.csv"))
            {
                int j = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    for (int i = 0; i < 64; i++)
                    {
                        A[j, i] = Int32.Parse(values[i]);
                    }
                    j++;
                }
            }

            List<int> indexs = new List<int>();
            int max = -1;
            int index = -1;
            int sum = 0;
            for (int i = 0; i < 3095; i++)
            {
                max = -1;
                sum = 0;
                index = -1;
                for (int j = 0; j < 64; j++)
                {
                    if (A[i, j] > max)
                    {
                        max = A[i, j];
                        index = j;
                    }
                    sum += A[i, j];
                }

                indexs.Add(index);
                for (int k = 0; k < 64; k++)
                {

                    if (k != index)
                    {
                        A[i, k] = -250;
                    }
                    else
                    {
                        A[i, k] = sum;
                    }
                }

            }

            var sb = new StringBuilder();

            for (int i = 0; i < 3095; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    sb.Append(A[i, j]);
                    if (j != 63)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();
            }


            await mLRepository.EditSellersClusters(indexs);


            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\a.csv", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> EncodingBinary()
        {
            int[,] A = new int[31900, 64];
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\binnary.csv"))
            {
                int j = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split('\t');

                    for (int i = 0; i < 64; i++)
                    {
                        A[j, i] = Int32.Parse(values[i]);
                    }
                    j++;
                }
            }

            int[,] B = new int[31900, 6];
            string[] C = new string[31900];
            string binary = "";
            for (int i = 0; i < 31900; i++)
            {
                for (int j = 0; j < 64; j++)
                {
                    if (A[i, j] == 1)
                    {
                        C[i] = Convert.ToString(j, 2);
                        break;
                    }
                }

            }

            for (int i = 0; i < 31900; i++)
            {
                for (int j = 0; j < C[i].Length; j++)
                {
                    B[i, j] = C[i][j] - '0';
                }
            }

            var sb = new StringBuilder();

            for (int i = 0; i < 31900; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    sb.Append(B[i, j]);
                    if (j != 5)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendLine();
            }

            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\aa.csv", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> FillCategoriesNumber()
        {
            await mLRepository.FillCategoriesNumber();
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFinalCsv()
        {
            await mLRepository.GenerateFinalCsv();
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateBinary()
        {
            var result = new List<int>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\FInaaaaaal.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<int>().ToList();
            }

            var sb = new StringBuilder();

            for (int i = 0; i < 32285; i++)
            {
                for (int j = 1; j < result[i]; j++)
                {
                    sb.Append("0,");
                }

                sb.Append("1,");

                for (int k = result[i] + 1; k <= 64; k++)
                {
                    sb.Append("0,");
                }
                sb.AppendLine();
            }

            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\Z.csv", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GenerateBinaryV2()
        {
            var result = new List<int>();
            using (var reader = new StreamReader(@$"C:\Users\Osama Al-Rashed\source\repos\SmartDistributor\SmartDistributor.Api\wwwroot\FInaaaaaal.csv", Encoding.Default))
            using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.CreateSpecificCulture("en-us")))
            {
                result = csv.GetRecords<int>().ToList();
            }

            var sb = new StringBuilder();

            for (int i = 0; i < 32285; i++)
            {
                //Random r = new Random(result[i]);
                //var bound = r.Next(1, 65);

                for (int j = 1; j < (result[i] == 64 ? 1 : result[i] + 1) ; j++)
                {
                    sb.Append("0,");
                }

                sb.Append("1,");

                for (int k = (result[i] == 64 ? 1 : result[i] + 1); k <= 64; k++)
                {
                    sb.Append("0,");
                }
                sb.AppendLine();
            }

            var text = sb.ToString();
            System.IO.File.WriteAllText(@"C:\Users\Osama Al-Rashed\Desktop\Z.csv", text);
            return new JsonResult("") { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> RemoveGeoLocationAndCity()
        {
            var result = await mLRepository.RemoveGeoLocationAndCity();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> FilterGeoLocationAndCity()
        {
            var result = await mLRepository.FilterGeoLocationAndCity();
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpPost]
        public async Task<IActionResult> AddGeoLocation(GeoLocationDto geoLocationDto)
        {
            var result = await mLRepository.AddGeoLocation(geoLocationDto);
            return new JsonResult(result) { StatusCode = 200 };
        }

        [HttpGet]
        public async Task<IActionResult> GetReBinaryV2()
        {
            int[] maps = new int[65];
            for (int i = 1; i <= 64; i++)
            {
                Random r = new Random(i);
                var bound = r.Next(1, 65);
                var bound2 = r.Next(1, 65);
                maps[bound] = i;
            }
            
            
            return new JsonResult(maps) { StatusCode = 200 };
        }


    }
}
