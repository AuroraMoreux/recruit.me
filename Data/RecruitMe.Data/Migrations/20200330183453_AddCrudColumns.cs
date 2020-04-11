namespace RecruitMe.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddCrudColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "Skills",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "Skills",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "Skills",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "Skills",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "Languages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "Languages",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "Languages",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "Languages",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "JobSectors",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "JobSectors",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "JobSectors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "JobSectors",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "JobLevels",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "JobLevels",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "JobLevels",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "JobLevels",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "FileExtensions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "FileExtensions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "FileExtensions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "FileExtensions",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "DocumentCategories",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "DocumentCategories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "DocumentCategories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "DocumentCategories",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                schema: "enum",
                table: "ApplicationStatuses",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                schema: "enum",
                table: "ApplicationStatuses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "enum",
                table: "ApplicationStatuses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                schema: "enum",
                table: "ApplicationStatuses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_IsDeleted",
                schema: "enum",
                table: "Skills",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_IsDeleted",
                schema: "enum",
                table: "Languages",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_JobSectors_IsDeleted",
                schema: "enum",
                table: "JobSectors",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_JobLevels_IsDeleted",
                schema: "enum",
                table: "JobLevels",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_FileExtensions_IsDeleted",
                schema: "enum",
                table: "FileExtensions",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentCategories_IsDeleted",
                schema: "enum",
                table: "DocumentCategories",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatuses_IsDeleted",
                schema: "enum",
                table: "ApplicationStatuses",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_IsDeleted",
                schema: "enum",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Languages_IsDeleted",
                schema: "enum",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_JobSectors_IsDeleted",
                schema: "enum",
                table: "JobSectors");

            migrationBuilder.DropIndex(
                name: "IX_JobLevels_IsDeleted",
                schema: "enum",
                table: "JobLevels");

            migrationBuilder.DropIndex(
                name: "IX_FileExtensions_IsDeleted",
                schema: "enum",
                table: "FileExtensions");

            migrationBuilder.DropIndex(
                name: "IX_DocumentCategories_IsDeleted",
                schema: "enum",
                table: "DocumentCategories");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStatuses_IsDeleted",
                schema: "enum",
                table: "ApplicationStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "JobSectors");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "JobSectors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "JobSectors");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "JobSectors");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "JobLevels");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "JobLevels");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "JobLevels");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "JobLevels");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "FileExtensions");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "FileExtensions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "FileExtensions");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "FileExtensions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "DocumentCategories");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "DocumentCategories");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "DocumentCategories");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "DocumentCategories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                schema: "enum",
                table: "ApplicationStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                schema: "enum",
                table: "ApplicationStatuses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "enum",
                table: "ApplicationStatuses");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                schema: "enum",
                table: "ApplicationStatuses");
        }
    }
}
