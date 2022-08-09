using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiStarter_Jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiStarter_Jobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiStarter_Makes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiStarter_Makes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiStarter_Models",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MakeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiStarter_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiStarter_Models_ApiStarter_Makes_MakeId",
                        column: x => x.MakeId,
                        principalTable: "ApiStarter_Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiStarter_Cars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearOfCreation = table.Column<int>(type: "int", nullable: false),
                    Modifier = table.Column<float>(type: "real", nullable: false),
                    ModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiStarter_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiStarter_Cars_ApiStarter_Models_ModelId",
                        column: x => x.ModelId,
                        principalTable: "ApiStarter_Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApiStarter_Cars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiStarter_Visits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckedIn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Completion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalPrice = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiStarter_Visits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiStarter_Visits_ApiStarter_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "ApiStarter_Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobVisit",
                columns: table => new
                {
                    JobsId = table.Column<int>(type: "int", nullable: false),
                    VisitsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobVisit", x => new { x.JobsId, x.VisitsId });
                    table.ForeignKey(
                        name: "FK_JobVisit_ApiStarter_Jobs_JobsId",
                        column: x => x.JobsId,
                        principalTable: "ApiStarter_Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobVisit_ApiStarter_Visits_VisitsId",
                        column: x => x.VisitsId,
                        principalTable: "ApiStarter_Visits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiStarter_Cars_ModelId",
                table: "ApiStarter_Cars",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiStarter_Cars_UserId",
                table: "ApiStarter_Cars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiStarter_Models_MakeId",
                table: "ApiStarter_Models",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiStarter_Visits_CarId",
                table: "ApiStarter_Visits",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_JobVisit_VisitsId",
                table: "JobVisit",
                column: "VisitsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobVisit");

            migrationBuilder.DropTable(
                name: "ApiStarter_Jobs");

            migrationBuilder.DropTable(
                name: "ApiStarter_Visits");

            migrationBuilder.DropTable(
                name: "ApiStarter_Cars");

            migrationBuilder.DropTable(
                name: "ApiStarter_Models");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ApiStarter_Makes");
        }
    }
}
