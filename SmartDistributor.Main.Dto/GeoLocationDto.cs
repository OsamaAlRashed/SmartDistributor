using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class GeoLocationDto
    {
        public int ZipCodePrefix { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string CityName { get; set; }
        public Guid CityId { get; set; }

    }
}
