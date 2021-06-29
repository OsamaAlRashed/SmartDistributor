using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int ProductsCount { get; set; }
    }
}
