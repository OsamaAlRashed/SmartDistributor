using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
