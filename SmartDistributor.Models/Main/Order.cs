using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Order : EntityBase
    {
        public DateTime PurchaseDate { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Cutomer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
