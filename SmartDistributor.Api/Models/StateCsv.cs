using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartDistributor.Api.Models
{
    public class StateCsv
    {
        [Name("name")]
        public string Name { get; set; }
    }
}
