using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidDataApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CaseTimeSeries",
                columns: table => new
                {
                    Date = table.Column<DateTime>(nullable: false),
                    Dailyconfirmed = table.Column<int>(nullable: false),
                    Dailydeceased = table.Column<int>(nullable: false),
                    Dailyrecovered = table.Column<int>(nullable: false),
                    Totalconfirmed = table.Column<int>(nullable: false),
                    Totaldeceased = table.Column<int>(nullable: false),
                    Totalrecovered = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseTimeSeries", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "CovidData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    CountryOrRegion = table.Column<string>(nullable: true),
                    ProvinceOrState = table.Column<string>(nullable: true),
                    Lat = table.Column<double>(nullable: false),
                    Long = table.Column<double>(nullable: false),
                    Confirmed = table.Column<long>(nullable: false),
                    Recovered = table.Column<long>(nullable: false),
                    Deaths = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovidData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Feedback = table.Column<string>(maxLength: 1000, nullable: false),
                    SubmitedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatesData",
                columns: table => new
                {
                    State = table.Column<string>(maxLength: 50, nullable: false),
                    Active = table.Column<long>(nullable: false),
                    Confirmed = table.Column<long>(nullable: false),
                    Deaths = table.Column<long>(nullable: false),
                    DeltaConfirmed = table.Column<long>(nullable: false),
                    DeltaDeaths = table.Column<long>(nullable: false),
                    DeltaRecovered = table.Column<long>(nullable: false),
                    LastUpdatedtime = table.Column<DateTime>(nullable: false),
                    Recovered = table.Column<long>(nullable: false),
                    Statecode = table.Column<string>(maxLength: 50, nullable: true),
                    Statenotes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatesData", x => x.State);
                });

            migrationBuilder.CreateTable(
                name: "TestData",
                columns: table => new
                {
                    Updatetimestamp = table.Column<string>(nullable: false),
                    Individualstestedperconfirmedcase = table.Column<double>(nullable: true),
                    Positivecasesfromsamplesreported = table.Column<double>(nullable: true),
                    Samplereportedtoday = table.Column<double>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    Testpositivityrate = table.Column<double>(nullable: true),
                    Testsconductedbyprivatelabs = table.Column<double>(nullable: true),
                    Testsperconfirmedcase = table.Column<double>(nullable: true),
                    Totalindividualstested = table.Column<double>(nullable: true),
                    Totalpositivecases = table.Column<double>(nullable: true),
                    Totalsamplestested = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestData", x => x.Updatetimestamp);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CaseTimeSeries");

            migrationBuilder.DropTable(
                name: "CovidData");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "StatesData");

            migrationBuilder.DropTable(
                name: "TestData");
        }
    }
}
