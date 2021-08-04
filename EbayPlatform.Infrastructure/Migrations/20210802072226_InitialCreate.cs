using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EbayPlatform.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncTaskJobConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    JobDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    JobClassFullName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Cron = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CronDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    IsRunning = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    JobStatus = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    ModifyDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTaskJobConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SyncTaskJobParam",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParamValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTaskJobParam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyncTaskJobParam_SyncTaskJobConfigs_Id",
                        column: x => x.Id,
                        principalTable: "SyncTaskJobConfigs",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncTaskJobParam");

            migrationBuilder.DropTable(
                name: "SyncTaskJobConfigs");
        }
    }
}
