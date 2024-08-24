using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class AddingBuidlings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 0; i < 30; i++)
            {
                migrationBuilder.Sql($"INSERT INTO [dbo].[Buildings] VALUES ('{Guid.NewGuid()}', 'b5f13106-f633-412c-a8fb-caceadd2d174' , '{i}')");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
