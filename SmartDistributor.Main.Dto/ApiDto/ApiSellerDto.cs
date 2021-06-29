using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto.ApiDto
{
    public class ApiSellerDto
    {
        public Guid Id { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
