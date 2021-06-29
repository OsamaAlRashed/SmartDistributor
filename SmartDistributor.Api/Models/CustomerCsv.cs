using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class CustomerCsv
    {
        [Name("customer_id")]
        public Guid Id { get; set; }

        [Name("customer_city")]
        public string CityName { get; set; }
    }
}
