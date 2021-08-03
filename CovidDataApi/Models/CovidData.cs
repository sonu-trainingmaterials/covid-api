using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    public class CovidData
    {       
        [Key]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string CountryOrRegion { get; set; }

        public string ProvinceOrState { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public long Confirmed { get; set; }

        public long Recovered { get; set; }

        public long Deaths { get; set; }
    }
}
