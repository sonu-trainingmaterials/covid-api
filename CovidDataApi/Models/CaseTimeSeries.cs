using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    [Table("CaseTimeSeries")]
    public class CaseTimeSeries
    {        
        public int  Dailyconfirmed { get; set; }
        public int  Dailydeceased { get; set; }
        public int  Dailyrecovered { get; set; }
        [Key]
        public DateTime Date { get; set; }
        public int Totalconfirmed { get; set; }
        public int Totaldeceased { get; set; }
        public int Totalrecovered { get; set; }
    }
}
