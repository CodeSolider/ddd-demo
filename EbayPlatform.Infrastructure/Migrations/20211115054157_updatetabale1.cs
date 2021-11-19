using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EbayPlatform.Infrastructure.Migrations
{
    public partial class updatetabale1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobStatus",
                table: "SyncTaskJobConfig");

            migrationBuilder.AlterColumn<bool>(
                name: "ShippingDetail_GetItFast",
                table: "Order",
                type: "bit",
                nullable: true,
                defaultValue: false,
                comment: "是否加急",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldComment: "是否加急");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JobStatus",
                table: "SyncTaskJobConfig",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "运行状态");

            migrationBuilder.AlterColumn<bool>(
                name: "ShippingDetail_GetItFast",
                table: "Order",
                type: "bit",
                nullable: true,
                comment: "是否加急",
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: false,
                oldComment: "是否加急");
        }
    }
}
