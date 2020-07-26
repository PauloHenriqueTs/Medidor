using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class dfbtrttjja : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HouseEnergyMeters");

            migrationBuilder.DropTable(
                name: "MeterOfPoleEnergyMeters");

            migrationBuilder.DropTable(
                name: "PoleEnergyMeters");

            migrationBuilder.CreateTable(
                name: "EnergyMetersDaos",
                columns: table => new
                {
                    SerialId = table.Column<string>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Count = table.Column<string>(nullable: false),
                    SwitchState = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnergyMetersDaos", x => x.SerialId);
                });

            migrationBuilder.CreateTable(
                name: "MeterOfPoleDaos",
                columns: table => new
                {
                    MeterOfPoleDaoId = table.Column<string>(nullable: false),
                    EnergyMeterDaoId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterOfPoleDaos", x => x.MeterOfPoleDaoId);
                    table.ForeignKey(
                        name: "FK_MeterOfPoleDaos_EnergyMetersDaos_EnergyMeterDaoId",
                        column: x => x.EnergyMeterDaoId,
                        principalTable: "EnergyMetersDaos",
                        principalColumn: "SerialId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterOfPoleDaos_EnergyMeterDaoId",
                table: "MeterOfPoleDaos",
                column: "EnergyMeterDaoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterOfPoleDaos");

            migrationBuilder.DropTable(
                name: "EnergyMetersDaos");

            migrationBuilder.CreateTable(
                name: "HouseEnergyMeters",
                columns: table => new
                {
                    HouseEnergyMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SwitchState = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouseEnergyMeters", x => x.HouseEnergyMeterId);
                });

            migrationBuilder.CreateTable(
                name: "PoleEnergyMeters",
                columns: table => new
                {
                    PoleEnergyMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerialId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoleEnergyMeters", x => x.PoleEnergyMeterId);
                });

            migrationBuilder.CreateTable(
                name: "MeterOfPoleEnergyMeters",
                columns: table => new
                {
                    MeterOfPoleEnergyMeterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoleEnergyMeterId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoleEnergyMeterId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterOfPoleEnergyMeters", x => x.MeterOfPoleEnergyMeterId);
                    table.ForeignKey(
                        name: "FK_MeterOfPoleEnergyMeters_PoleEnergyMeters_PoleEnergyMeterId1",
                        column: x => x.PoleEnergyMeterId1,
                        principalTable: "PoleEnergyMeters",
                        principalColumn: "PoleEnergyMeterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterOfPoleEnergyMeters_PoleEnergyMeterId1",
                table: "MeterOfPoleEnergyMeters",
                column: "PoleEnergyMeterId1");
        }
    }
}
