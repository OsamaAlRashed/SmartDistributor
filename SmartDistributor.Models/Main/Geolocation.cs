using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Geolocation : EntityBase
    {
        public int ZipCodePrefix { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
        public Guid CityId { get; set; }


    }
}
