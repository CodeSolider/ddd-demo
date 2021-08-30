using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EbayPlatform.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyncTaskJobConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Job名称"),
                    JobDesc = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Job描述"),
                    JobClassFullName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false, comment: "Jo类全名"),
                    Cron = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Cron"),
                    CronDesc = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, comment: "Cron描述"),
                    IsRunning = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "是否运行中"),
                    JobStatus = table.Column<int>(type: "int", nullable: false, comment: "运行状态"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建日期"),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "更新日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTaskJobConfig", x => x.Id);
                },
                comment: "同步任务作业配置表");

            migrationBuilder.CreateTable(
                name: "SystemLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "主键Id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ObjectId = table.Column<int>(type: "int", nullable: false, comment: "外键Id"),
                    LogType = table.Column<int>(type: "int", nullable: false, comment: "日志类型"),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, comment: "内容"),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "创建日期")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLog", x => x.Id);
                },
                comment: "系统日志表");

            migrationBuilder.CreateTable(
                name: "SyncTaskJobParam",
                columns: table => new
                {
                    SyncTaskJobConfigId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "店铺名称"),
                    ParamValue = table.Column<string>(type: "nvarchar(max)", nullable: true, comment: "参数值")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncTaskJobParam", x => new { x.SyncTaskJobConfigId, x.Id });
                    table.ForeignKey(
                        name: "FK_SyncTaskJobParam_SyncTaskJobConfig_SyncTaskJobConfigId",
                        column: x => x.SyncTaskJobConfigId,
                        principalTable: "SyncTaskJobConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncTaskJobParam");

            migrationBuilder.DropTable(
                name: "SystemLog");

            migrationBuilder.DropTable(
                name: "SyncTaskJobConfig");
        }
    }
}
