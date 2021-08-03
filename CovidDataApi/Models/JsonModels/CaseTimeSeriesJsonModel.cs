using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models.JsonModels
{
    public class CaseTimeSeriesJsonModel
    {
        public int Dailyconfirmed { get; set; }
        public int Dailydeceased { get; set; }
        public int Dailyrecovered { get; set; }        
        public string Date { get; set; }
        public int Totalconfirmed { get; set; }
        public int Totaldeceased { get; set; }
        public int Totalrecovered { get; set; }
    }
}
