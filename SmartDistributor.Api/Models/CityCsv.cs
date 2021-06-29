using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class CityCsv
    {
        [Name("city")]
        public string City { get; set; }
        [Name("state")]
        public string State { get; set; }
    }
}
