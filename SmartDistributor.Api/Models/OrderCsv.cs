using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class OrderCsv
    {
        [Name("id")]
        public Guid Id { get; set; }

        [Name("customer_id")]
        public Guid CustomerId { get; set; }

        [Name("purchaseDate")]
        public DateTime PurchaseDate { get; set; }
    }
}
