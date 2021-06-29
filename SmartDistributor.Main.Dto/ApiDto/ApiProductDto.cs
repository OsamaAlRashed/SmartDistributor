using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto.ApiDto
{
    public class ApiProductDto
    {
        public Guid Id { get; set; }
        public int CategoryNumber { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public double Price { get; set; }
        public double TotalOfPaid { get; set; }
        public Guid CategoryId { get; set; }
    }
}
