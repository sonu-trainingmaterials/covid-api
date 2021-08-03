using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidDataApi.Models
{
    public class CovidDbContext:DbContext
    {
        public CovidDbContext(DbContextOptions<CovidDbContext> options):base(options)
        {

        }

        public DbSet<CovidData> CovidData { get; set; }
        public DbSet<FeedbackData> Feedbacks { get; set; }
        public DbSet<StateData> StatesData { get; set; }
        public DbSet<TestData> TestData { get; set; }
        public DbSet<CaseTimeSeries> CaseTimeSeries { get; set; }
    }
}
