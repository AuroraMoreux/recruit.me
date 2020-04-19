using Microsoft.EntityFrameworkCore.Migrations;

namespace RecruitMe.Data.Migrations
{
    public partial class RenameTitleToPosition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "JobOffers");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "JobOffers",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "JobOffers");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "JobOffers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
