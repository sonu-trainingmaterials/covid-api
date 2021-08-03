using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidDataApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace CovidDataApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CovidDbContext>(options =>
            {
                //options.UseInMemoryDatabase("CovidDb");
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection"));
            });
            services.AddSingleton<CovidDataProcessor>();
            services.AddSingleton<IndiaDataProcessor>();

            services.AddCors(c=>
            {
                c.AddDefaultPolicy(config =>
                {
                    config.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title="Covid Data Servcie",
                    Version="1.0",
                    Contact=new OpenApiContact
                    {
                        Email="sonusathyadas@gmail.com",
                        Name="Sonu Sathyadas"
                    }
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, CovidDbContext db, 
            CovidDataProcessor csvc, IndiaDataProcessor isvc)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            InitializeDatabase(app, db);            

            csvc.LoadData(db);
            isvc.LoadData(db);

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CovidAPI");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void InitializeDatabase(IApplicationBuilder app, CovidDbContext db)
        {
            using(var serviceScope=app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<CovidDbContext>().Database.Migrate();
                CovidDataProcessor.GetData(db).GetAwaiter().GetResult();
                IndiaDataProcessor.GetData(db).GetAwaiter().GetResult(); ;
            }
        }
    }
}
