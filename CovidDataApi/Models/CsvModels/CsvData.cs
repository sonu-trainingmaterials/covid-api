using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    public class CsvData
    {

            public DateTime Date { get; set; }

            public string CountryOrRegion { get; set; }

            public string ProvinceOrState { get; set; }

            public string Confirmed { get; set; }

            public string Recovered { get; set; }

            public string Deaths { get; set; }
        
    }
}
