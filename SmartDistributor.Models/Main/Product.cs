using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Product : EntityBase
    {
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }

        public bool IsChoose { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
