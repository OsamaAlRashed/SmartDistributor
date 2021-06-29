using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Category : EntityBase
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public bool IsIgnored { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
