using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class SellerProductsCountDto
    {
        public Guid SellerId { get; set; }
        public int Count { get; set; }
    }
}
