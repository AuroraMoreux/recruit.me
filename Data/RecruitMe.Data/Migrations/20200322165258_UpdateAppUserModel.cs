namespace RecruitMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class UpdateAppUserModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "Candidates");

            migrationBuilder.DropColumn(
                name: "IsCandidate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsEmployer",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                table: "Employers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Candidates",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoUrl",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Candidates");

            migrationBuilder.AddColumn<string>(
                name: "Logo",
                table: "Employers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePicture",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCandidate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmployer",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
