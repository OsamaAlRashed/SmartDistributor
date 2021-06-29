using SmartDistributor.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Models.Main
{
    public class State : EntityBase
    {
        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}