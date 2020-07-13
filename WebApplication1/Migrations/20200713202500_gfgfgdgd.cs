using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class gfgfgdgd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterOfPoleEnergyMeters_PoleEnergyMeters_PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PoleEnergyMeters",
                table: "PoleEnergyMeters");

            migrationBuilder.DropIndex(
                name: "IX_MeterOfPoleEnergyMeters_PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "SerialId",
                table: "PoleEnergyMeters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "PoleEnergyMeterId",
                table: "PoleEnergyMeters",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HouseEnergyMeterId",
                table: "HouseEnergyMeters",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PoleEnergyMeters",
                table: "PoleEnergyMeters",
                column: "PoleEnergyMeterId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterOfPoleEnergyMeters_PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters",
                column: "PoleEnergyMeterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterOfPoleEnergyMeters_PoleEnergyMeters_PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters",
                column: "PoleEnergyMeterId1",
                principalTable: "PoleEnergyMeters",
                principalColumn: "PoleEnergyMeterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterOfPoleEnergyMeters_PoleEnergyMeters_PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PoleEnergyMeters",
                table: "PoleEnergyMeters");

            migrationBuilder.DropIndex(
                name: "IX_MeterOfPoleEnergyMeters_PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropColumn(
                name: "PoleEnergyMeterId",
                table: "PoleEnergyMeters");

            migrationBuilder.DropColumn(
                name: "PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropColumn(
                name: "HouseEnergyMeterId",
                table: "HouseEnergyMeters");

            migrationBuilder.AlterColumn<string>(
                name: "SerialId",
                table: "PoleEnergyMeters",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PoleEnergyMeters",
                table: "PoleEnergyMeters",
                column: "SerialId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterOfPoleEnergyMeters_PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters",
                column: "PoleEnergyMeterId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterOfPoleEnergyMeters_PoleEnergyMeters_PoleEnergyMeterId",
                table: "MeterOfPoleEnergyMeters",
                column: "PoleEnergyMeterId",
                principalTable: "PoleEnergyMeters",
                principalColumn: "SerialId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
