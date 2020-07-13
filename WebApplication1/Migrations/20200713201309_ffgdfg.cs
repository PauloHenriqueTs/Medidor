using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class ffgdfg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterOfPoleEnergyMeters",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "MeterId",
                table: "MeterOfPoleEnergyMeters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "MeterOfPoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterOfPoleEnergyMeters",
                table: "MeterOfPoleEnergyMeters",
                column: "MeterOfPoleEnergyMeterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterOfPoleEnergyMeters",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropColumn(
                name: "MeterOfPoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "MeterId",
                table: "MeterOfPoleEnergyMeters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterOfPoleEnergyMeters",
                table: "MeterOfPoleEnergyMeters",
                column: "MeterId");
        }
    }
}
