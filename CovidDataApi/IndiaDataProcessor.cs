using CovidDataApi.Models;
using CovidDataApi.Models.CsvModels;
using CovidDataApi.Models.JsonModels;
using CsvHelper;
using EFCore.BulkExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CovidDataApi
{
    public class IndiaDataProcessor
    {
        private static string baseUrl = "https://api.covid19india.org/";
        private static string dataFile = "data.json";
        private static HttpClient http;
        public List<StateData> StatesData { get; private set; }
        public List<TestData> TestData { get; private set; }
        public List<CaseTimeSeries> CaseTimeSeries { get; private set; }

        public async static Task<bool> GetData(CovidDbContext dbc)
        {
            http = new HttpClient();
            http.BaseAddress = new Uri(baseUrl);
            var data = await http.GetStringAsync(dataFile);
            data = data.Replace("cases_time_series", "CasesTimeSeries");
            data = data.Replace("%", string.Empty);
            TextReader reader = new StringReader(data);            
            var jsonSerializer = new JsonSerializer();
            try
            {
                dbc.StatesData.BatchDelete();
                dbc.TestData.BatchDelete(); 
                dbc.CaseTimeSeries.BatchDelete();                
                var records=jsonSerializer.Deserialize(reader, typeof(StatewiseCombined)) as StatewiseCombined;
                
                var statedata = ConvertStateData(records.Statewise.ToList());
                var casestimeseries = ConvertCaseTimeSeriesData(records.CasesTimeSeries.ToList());
                var tested = records.Tested.ToList();
                dbc.BulkInsert(statedata);
                dbc.BulkInsert(casestimeseries);
                dbc.BulkInsert(tested);
                Console.WriteLine("States data inserted");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void LoadData(CovidDbContext db)
        {
            StatesData= db.StatesData.ToList();
            TestData=db.TestData.ToList();
            CaseTimeSeries=db.CaseTimeSeries.ToList();
        }

        private static List<StateData> ConvertStateData(IEnumerable<StateDataJsonModel> items)
        {
            return items                
                .Select(s => new StateData()
            {
                Active=s.Active,
                Confirmed = s.Confirmed,
                Deaths= s.Deaths,
                DeltaConfirmed=s.DeltaConfirmed,
                DeltaDeaths=s.DeltaDeaths,
                DeltaRecovered=s.DeltaRecovered,
                Recovered=s.Recovered,
                Statecode=s.Statecode,
                State=s.State,
                Statenotes=s.Statenotes,
                LastUpdatedtime=DateTime.ParseExact(s.LastUpdatedtime.Substring(0,10),"dd/MM/yyyy",null)
            }).ToList();
            
        }

        private static List<CaseTimeSeries> ConvertCaseTimeSeriesData(IEnumerable<CaseTimeSeriesJsonModel> items)
        {
            return items.Select(s=>new CaseTimeSeries()
            {
                Dailyconfirmed=s.Dailyconfirmed,
                Dailydeceased=s.Dailydeceased,
                Dailyrecovered=s.Dailyrecovered,
                Totalconfirmed=s.Totalconfirmed,
                Totaldeceased=s.Totaldeceased,
                Totalrecovered=s.Totalrecovered,
                Date= GetConvertedDate(s.Date)
            }).ToList();
        }

        private static DateTime GetConvertedDate(string date)
        {
            Dictionary<string, string> months = new Dictionary<string, string>()
            {
                { "january", "01"},{ "february", "02"},{ "march", "03"},{ "april", "04"},
                { "may", "05"},{ "june", "06"},{ "july", "07"},{ "august", "08"},
                { "september", "09"},{ "october", "10"},{ "november", "11"},{ "december", "12"}
            };
            var day = date.Substring(0, date.IndexOf(' '));
            var monthText = date.Substring(date.IndexOf(' ')+1).Trim();
            var month = months[monthText.ToLower()];
            var dateText = string.Format("{0}/{1}/{2}", month, day, "2020");
            return DateTime.ParseExact(dateText,"MM/dd/yyyy",null);
        }
    }
}
