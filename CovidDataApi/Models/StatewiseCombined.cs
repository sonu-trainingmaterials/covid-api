using CovidDataApi.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models.CsvModels
{
    public class StatewiseCombined
    {

        public IEnumerable<CaseTimeSeriesJsonModel> CasesTimeSeries { get; set; }
        public IEnumerable<StateDataJsonModel> Statewise { get; set; }
        public IEnumerable<TestData> Tested { get; set; }
             
    }
}
