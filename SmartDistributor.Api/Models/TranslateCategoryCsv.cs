using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class TranslateCategoryCsv
    {
        [Name("categoryName")]
        public string CategoryName { get; set; }

        [Name("categoryNameEnglish")]
        public string CategoryNameEnglish { get; set; }
    }
}
