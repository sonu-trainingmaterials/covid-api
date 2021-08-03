using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidDataApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CovidDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidController : ControllerBase
    {
        private CovidDbContext db;        
        //private static IEnumerable<CovidData> covidData = new List<CovidData>();
        private CovidDataProcessor covidSvc;
        private IndiaDataProcessor indiaSvc;

        public CovidController(CovidDbContext dbContext, CovidDataProcessor covidService, IndiaDataProcessor isvc)
        {
            db = dbContext;
            covidSvc = covidService;
            indiaSvc = isvc;
        }

        [HttpGet("data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CovidData>> GetCovidData([FromQuery]int days = 1)
        {
            var sdate = covidSvc.CovidData.Max(s => s.Date);
            sdate = sdate.AddDays(-(days-1));

            //var sdate = DateTime.Now.AddHours(-(days*24));
            var result = covidSvc.CovidData
                .OrderByDescending(s=>s.Date)
                .Where(s => s.Date >= sdate)
                .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date);                
            return Ok(result);
        }

        [HttpGet("country/{country}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CovidData>> GetCovidDataByCountry(string country, [FromQuery]int? days)
        {
            //if (covidData.Count() == 0)
            //{
            //    var data = await db.CovidData
            //        .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date)
            //        .ToListAsync();
            //    covidData = data;
            //}
            
            var result = covidSvc.CovidData.Where(s => s.CountryOrRegion.ToLower()==country.ToLower())
                .OrderBy(s=>s.Date)
                .ToList();
            if (days.HasValue)
            {
                var sdate = DateTime.Now.AddDays(-(days.Value + 1));
                result = result.Where(s => s.Date >= sdate).ToList();
            }
            return Ok(result);
        }

        [HttpGet("date/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CovidData>> GetCovidDataByDate(DateTime date)
        {
            //if (covidData.Count() == 0)
            //{
            //    var data = await db.CovidData
            //        .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date)
            //        .ToListAsync();
            //    covidData = data;
            //}
            var result = covidSvc.CovidData.Where(s => s.Date.Month == date.Month && s.Date.Day == date.Day && s.Date.Year == date.Year);                
            return Ok(result);
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<IEnumerable< CovidData>> GetCovidDataByCountryAndDate([FromQuery]string country, [FromQuery] DateTime? date)
        {
            //if (covidData.Count() == 0)
            //{
            //    var data = await db.CovidData
            //        .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date)
            //        .ToListAsync();
            //    covidData = data;
            //}
            var result = covidSvc.CovidData;
            if (!string.IsNullOrEmpty(country))
            {
                result = result.Where(s => s.CountryOrRegion.ToLower() == country.ToLower());
            }
            if (date.HasValue)
            {
                result = result.Where(s => s.Date.Month == date?.Month && s.Date.Day == date?.Day && s.Date.Year == date?.Year);
            }            
            if(string.IsNullOrEmpty(country) && !date.HasValue)
            {
                var sdate = DateTime.Now.AddDays(-2);
                result = covidSvc.CovidData
                    .OrderByDescending(s => s.Date)
                    .Where(s => s.Date >= sdate)
                    .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date);
            }
            return Ok(result);
        }

        [HttpGet("countries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetCountries()
        {
            //if (covidData.Count() == 0)
            //{
            //    var data = await db.CovidData
            //        .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date)
            //        .ToListAsync();
            //    covidData = data;
            //}
            var result = covidSvc.CovidData.Select(s => s.CountryOrRegion).Distinct();
            return Ok(result);
        }

        [HttpGet("dates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetDates()
        {
            //if (covidData.Count() == 0)
            //{
            //    var data = await db.CovidData
            //        .OrderBy(s => s.CountryOrRegion).ThenBy(s => s.Date)
            //        .ToListAsync();
            //    covidData = data;
            //}
            var result = covidSvc.CovidData.Select(s => s.Date).Distinct();
            return Ok(result.OrderByDescending(s=>s));
        }

        //Statewise data
        [HttpGet("india/statedata")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<StateData>> GetStatesData([FromQuery]DateTime? date)
        {
            if (date.HasValue)
            {
                var d = date.Value;
                return indiaSvc.StatesData
                    .Where(s => s.LastUpdatedtime.Month == d.Month && s.LastUpdatedtime.Day == d.Day && s.LastUpdatedtime.Year == d.Year)
                    .OrderByDescending(s => s.LastUpdatedtime)
                    .ToList();
            }
            return indiaSvc.StatesData
                .OrderByDescending(s=>s.LastUpdatedtime)
                .ToList();
        }

        [HttpGet("india/statedata/dates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DateTime>> GetDatesForStatesData()
        {
            return indiaSvc.StatesData.Select(s => s.LastUpdatedtime).Distinct().ToList();
        }

        //Timeseries data
        [HttpGet("india/cases/dates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<DateTime>> GetDatesForCaseTimeSeries()
        {
            return indiaSvc.CaseTimeSeries.Select(s => s.Date).Distinct()
                .OrderByDescending(s=>s)
                .ToList();
        }

        [HttpGet("india/cases")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<CaseTimeSeries>> GetCases([FromQuery]DateTime? date)
        {
            if (!date.HasValue)
                return indiaSvc.CaseTimeSeries
                    .OrderByDescending(s=>s.Date)
                    .ToList();
            else
            {
                var d = date.Value;
                return indiaSvc.CaseTimeSeries
                    .Where(s => s.Date.Month == d.Month && s.Date.Day == d.Day && s.Date.Year == d.Year)
                    .ToList();
            }
        }


    }
}