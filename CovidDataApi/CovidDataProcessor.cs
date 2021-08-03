using CovidDataApi.Models;
using CsvHelper;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CovidDataApi
{
    public class CovidDataProcessor
    {
        private static string baseUrl = "https://raw.githubusercontent.com/datasets/covid-19/main/data/";
        private static string dataFile = "time-series-19-covid-combined.csv";
        private static HttpClient http;
        public IEnumerable<CovidData> CovidData { get; set; }
       

        public static async Task<bool> GetData(CovidDbContext dbc)
        {
            http = new HttpClient();
            http.BaseAddress = new Uri(baseUrl);
            var data =await http.GetStringAsync(dataFile);
            data=data.Replace("Country/Region", "CountryOrRegion");
            data=data.Replace("Province/State", "ProvinceOrState");
            TextReader reader = new StringReader(data);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<CsvData>().ToList();            
            try
            {
                List<CovidData> items = new List<CovidData>();
                dbc.CovidData.BatchDelete();

                foreach (var rec in records)
                {
                    var d = new CovidData
                    {
                        CountryOrRegion= rec.CountryOrRegion,
                        ProvinceOrState = rec.ProvinceOrState,
                        Confirmed= string.IsNullOrEmpty(rec.Confirmed)?0:Int64.Parse(rec.Confirmed),
                        Deaths = string.IsNullOrEmpty(rec.Deaths) ? 0 : Int64.Parse(rec.Deaths),
                        Date = rec.Date,
                        Recovered= string.IsNullOrEmpty(rec.Recovered) ? 0 : Int64.Parse(rec.Recovered),
                    };
                    items.Add(d);
                }
                dbc.BulkInsert(items);
                Console.WriteLine(records.Count() + " records inserted to CovidData");
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
            CovidData= db.CovidData.ToList();
        }
    }
}
