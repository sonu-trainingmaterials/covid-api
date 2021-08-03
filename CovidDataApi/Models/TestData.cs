using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    [Table("TestData")]
    public class TestData
    {
        public double? Individualstestedperconfirmedcase { get;set; }
        public double? Positivecasesfromsamplesreported { get; set; }
        public double? Samplereportedtoday { get; set; }
        public string Source { get; set; }
        public double? Testpositivityrate { get; set; }
        public double? Testsconductedbyprivatelabs { get; set; }
        public double? Testsperconfirmedcase { get; set; }
        public double? Totalindividualstested { get; set; }
        public double? Totalpositivecases { get; set; }
        public double? Totalsamplestested { get; set; }
        [Key]
        public string Updatetimestamp { get; set; }
    }
}
