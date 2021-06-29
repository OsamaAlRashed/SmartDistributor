using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
        public double FreightValue { get; set; }
        public Guid ProductId { get; set; }
        public Guid SellerId { get; set; }
        public Guid OrderId { get; set; }
    }
}
