using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class AddingStreetsData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string[] StreetsNames =
            {
                "1",
                "5",
                "25",
                "15",
                "16",
                "18",
                "20",
                "b5f13106-f633-412c-a8fb-caceadd2d174"
            };
            for (int i = 0; i < StreetsNames.Length; i++)
            {
                migrationBuilder.Sql($"INSERT INTO [dbo].[Streets] VALUES ('{Guid.NewGuid()}', '8036127d-025b-4062-8c68-1b97f47fbd52' , '{StreetsNames[i]}')");
            }
            migrationBuilder.Sql($"INSERT INTO [dbo].[Streets] VALUES ('b5f13106-f633-412c-a8fb-caceadd2d174', '8036127d-025b-4062-8c68-1b97f47fbd52' , '45')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
