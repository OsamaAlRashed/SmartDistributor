using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Seller : EntityBase
    {
        public int ZipCodePrefix { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        //[ForeignKey(nameof(CityId))]
        //public City City { get; set; }
        //public int CityId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public int ClusterId { get; set; }
    }
}
