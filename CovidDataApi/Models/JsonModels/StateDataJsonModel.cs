using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models.JsonModels
{
    public class StateDataJsonModel
    {
        public long Active { get; set; }
        public long Confirmed { get; set; }
        public long Deaths { get; set; }
        public long DeltaConfirmed { get; set; }
        public long DeltaDeaths { get; set; }
        public long DeltaRecovered { get; set; }
        public string LastUpdatedtime { get; set; }
        public long Recovered { get; set; }        
        public string State { get; set; }        
        public string Statecode { get; set; }
        public string Statenotes { get; set; }
    }
}
