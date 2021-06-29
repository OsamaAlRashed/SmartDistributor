using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class SellerCsv
    {
        [Name("seller_id")]
        public Guid Id { get; set; }

        [Name("seller_zip_code_prefix")]
        public int ZipCodePrefix { get; set; }

        [Name("seller_city")]
        public string City { get; set; }

        [Name("seller_state")]
        public string State { get; set; }
    }
}
