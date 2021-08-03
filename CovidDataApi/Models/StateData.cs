using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    [Table("StatesData")]
    public class StateData
    {
        public long Active { get; set; }
        public long Confirmed { get; set; }
        public long Deaths { get; set; }
        public long DeltaConfirmed { get; set; }
        public long DeltaDeaths { get; set; }
        public long DeltaRecovered { get; set; }        
        public DateTime LastUpdatedtime { get; set; }
        public long Recovered { get; set; }
        [StringLength(50)]
        [Key]
        public string State { get; set; }
        [StringLength(50)]
        public string Statecode { get; set; }        
        public string Statenotes { get; set; }
    }
}
