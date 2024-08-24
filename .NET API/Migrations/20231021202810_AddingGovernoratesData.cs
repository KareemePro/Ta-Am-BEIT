using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodDelivery.Migrations
{
    public partial class AddingGovernoratesData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('cf9bcb15-258e-48ba-a9f6-fd1767413b46', 'Alexandria')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Aswan')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Asyut')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Beheira')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Beni Suef')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Cairo')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Dakahlia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Damietta')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Faiyum')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Gharbia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Giza')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Ismailia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Kafr El Sheikh')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Luxor')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Matrouh')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Minya')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Monufia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'New Valley')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'North Sinai')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Port Said')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Qalyubia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Red Sea')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Sharqia')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Sohag')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'South Sinai')");
            migrationBuilder.Sql($"INSERT INTO [dbo].[Governorates] VALUES ('{Guid.NewGuid()}', 'Suez')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
