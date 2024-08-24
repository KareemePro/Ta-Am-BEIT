using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class AddingDistrictsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] DistrictsNames =
                {
                "Dekhela",
                "Amreya 1",
                "Amreya 2",
                "Ataren",
                "Gomrok",
                "Labban",
                "Mansheya",
                "Montaza 1",
                "Montaza 2",
                "El Raml 1",
                "El Raml 2",
                "North Coast",
                "Bab Sharqi",
                "Borg El Arab",
                "Karmouz",
                "New Borg El Arab",
                "Port al-Basal",
                "Moharam Bek"
            };
            migrationBuilder.Sql($"INSERT INTO [dbo].[Districts] VALUES ('8036127d-025b-4062-8c68-1b97f47fbd52', 'cf9bcb15-258e-48ba-a9f6-fd1767413b46' , 'Sidi Gaber')");
            for (int i = 0; i < DistrictsNames.Length; i++)
            {
                migrationBuilder.Sql($"INSERT INTO [dbo].[Districts] VALUES ('{Guid.NewGuid()}', 'cf9bcb15-258e-48ba-a9f6-fd1767413b46' , '{DistrictsNames[i]}')");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
