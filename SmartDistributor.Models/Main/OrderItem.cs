using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;

namespace SmartDistributor.Models.Main
{
    public class OrderItem : EntityBase
    {
        public double Price { get; set; }
        public double FreightValue { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }

        public Guid SellerId { get; set; }
        public Seller Seller { get; set; }
    }
}
