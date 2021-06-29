using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto.ApiDto
{
    public class SellersPointDto
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public List<Guid> Ids { get; set; }
    }
}
