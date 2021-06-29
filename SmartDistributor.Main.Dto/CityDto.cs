using System;
using System.Collections.Generic;
using System.Text;

namespace SmartDistributor.Main.Dto
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid StateId { get; set; }
        public string StateName { get; set; }
    }
}
