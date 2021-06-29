using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class ProductCsv
    {
        [Name("id")]
        public Guid Id { get; set; }

        [Name("categoryname")]
        public string Name { get; set; }

        [Name("weight")]
        public int Weight { get; set; }

        [Name("length")]
        public int Length { get; set; }

        [Name("height")]
        public int Height { get; set; }

        [Name("width")]
        public int Width { get; set; }

    }
}
