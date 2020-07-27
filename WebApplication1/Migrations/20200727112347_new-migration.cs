using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class newmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterOfPoleDaos_EnergyMetersDaos_EnergyMeterDaoId",
                table: "MeterOfPoleDaos");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterOfPoleDaos_EnergyMetersDaos_EnergyMeterDaoId",
                table: "MeterOfPoleDaos",
                column: "EnergyMeterDaoId",
                principalTable: "EnergyMetersDaos",
                principalColumn: "SerialId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterOfPoleDaos_EnergyMetersDaos_EnergyMeterDaoId",
                table: "MeterOfPoleDaos");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterOfPoleDaos_EnergyMetersDaos_EnergyMeterDaoId",
                table: "MeterOfPoleDaos",
                column: "EnergyMeterDaoId",
                principalTable: "EnergyMetersDaos",
                principalColumn: "SerialId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
