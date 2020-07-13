using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class eweqwee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HouseEnergyMeters",
                table: "HouseEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "SerialId",
                table: "HouseEnergyMeters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HouseEnergyMeters",
                table: "HouseEnergyMeters",
                column: "HouseEnergyMeterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HouseEnergyMeters",
                table: "HouseEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "SerialId",
                table: "HouseEnergyMeters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_HouseEnergyMeters",
                table: "HouseEnergyMeters",
                column: "SerialId");
        }
    }
}
