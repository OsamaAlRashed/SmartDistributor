using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class OrderItemCsv
    {
        [Name("order_id")]
        public Guid OrderId { get; set; }

        [Name("product_id")]
        public Guid ProductId { get; set; }

        [Name("seller_id")]
        public Guid SellerId { get; set; }

        [Name("price")]
        public double Price { get; set; }

        [Name("freight_value")]
        public double FreightValue { get; set; }
    }
}
