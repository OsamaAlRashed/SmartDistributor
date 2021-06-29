using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class GeoLocationCsv
    { 
        [Name("zip_code_prefix")]
        public int ZipCodePrefix { get; set; }

        [Name("lat")]
        public double Lat { get; set; }

        [Name("lng")]
        public double Lng { get; set; }

        [Name("city")]
        public string CityName { get; set; }

    }
}
