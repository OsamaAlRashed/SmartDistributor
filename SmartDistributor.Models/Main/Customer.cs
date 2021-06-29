using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class Customer : EntityBase
    {
        //public int ZipCodePrefix { get; set; }

        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
        public Guid CityId { get; set; }


        public ICollection<Order> Orders { get; set; }

    }
}
