using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class City : EntityBase
    {
        public string Name { get; set; }

        [ForeignKey(nameof(StateId))]
        public State State { get; set; }
        public Guid StateId { get; set; }
        public ICollection<Geolocation> Geolocations { get; set; }
        public ICollection<Customer> Customers { get; set; }
    }
}
