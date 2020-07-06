using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Data.Migrations
{
    public partial class test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MeterOfPole",
                columns: table => new
                {
                    meterSerialId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    poleSerialId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterOfPole", x => x.meterSerialId);
                    table.ForeignKey(
                        name: "FK_MeterOfPole_EnergyMeters_poleSerialId",
                        column: x => x.poleSerialId,
                        principalTable: "EnergyMeters",
                        principalColumn: "serialId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterOfPole_poleSerialId",
                table: "MeterOfPole",
                column: "poleSerialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterOfPole");
        }
    }
}
