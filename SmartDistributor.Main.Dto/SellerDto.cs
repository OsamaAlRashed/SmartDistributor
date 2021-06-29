using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class SellerDto
    {
        public Guid Id { get; set; }
        public int ZipCodePrefix { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
