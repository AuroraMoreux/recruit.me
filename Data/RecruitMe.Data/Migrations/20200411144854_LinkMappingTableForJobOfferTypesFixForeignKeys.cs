using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RecruitMe.Data.Migrations
{
    public partial class LinkMappingTableForJobOfferTypesFixForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOffers_JobTypes_JobTypeId",
                table: "JobOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOfferSkills",
                table: "JobOfferSkills");

            migrationBuilder.DropIndex(
                name: "IX_JobOfferSkills_JobOfferId",
                table: "JobOfferSkills");

            migrationBuilder.DropIndex(
                name: "IX_JobOffers_JobTypeId",
                table: "JobOffers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOfferLanguages",
                table: "JobOfferLanguages");

            migrationBuilder.DropIndex(
                name: "IX_JobOfferLanguages_JobOfferId",
                table: "JobOfferLanguages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateSkills",
                table: "CandidateSkills");

            migrationBuilder.DropIndex(
                name: "IX_CandidateSkills_CandidateId",
                table: "CandidateSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateLanguages",
                table: "CandidateLanguages");

            migrationBuilder.DropIndex(
                name: "IX_CandidateLanguages_CandidateId",
                table: "CandidateLanguages");

            migrationBuilder.DropColumn(
                name: "JobTypeId",
                table: "JobOffers");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "JobOfferSkills",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "JobOffers",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "JobOfferLanguages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CandidateSkills",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CandidateLanguages",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOfferSkills",
                table: "JobOfferSkills",
                columns: new[] { "JobOfferId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOfferLanguages",
                table: "JobOfferLanguages",
                columns: new[] { "JobOfferId", "LanguageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateSkills",
                table: "CandidateSkills",
                columns: new[] { "CandidateId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateLanguages",
                table: "CandidateLanguages",
                columns: new[] { "CandidateId", "LanguageId" });

            migrationBuilder.CreateTable(
                name: "JobOfferJobTypes",
                columns: table => new
                {
                    JobOfferId = table.Column<string>(nullable: false),
                    JobTypeId = table.Column<int>(nullable: false),
                    Id = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedOn = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobOfferJobTypes", x => new { x.JobOfferId, x.JobTypeId });
                    table.ForeignKey(
                        name: "FK_JobOfferJobTypes_JobOffers_JobOfferId",
                        column: x => x.JobOfferId,
                        principalTable: "JobOffers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobOfferJobTypes_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalSchema: "enum",
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferJobTypes_IsDeleted",
                table: "JobOfferJobTypes",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferJobTypes_JobTypeId",
                table: "JobOfferJobTypes",
                column: "JobTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobOfferJobTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOfferSkills",
                table: "JobOfferSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobOfferLanguages",
                table: "JobOfferLanguages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateSkills",
                table: "CandidateSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidateLanguages",
                table: "CandidateLanguages");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "JobOfferSkills",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "JobOffers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AddColumn<int>(
                name: "JobTypeId",
                table: "JobOffers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "JobOfferLanguages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CandidateSkills",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "CandidateLanguages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOfferSkills",
                table: "JobOfferSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobOfferLanguages",
                table: "JobOfferLanguages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateSkills",
                table: "CandidateSkills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidateLanguages",
                table: "CandidateLanguages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferSkills_JobOfferId",
                table: "JobOfferSkills",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOffers_JobTypeId",
                table: "JobOffers",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOfferLanguages_JobOfferId",
                table: "JobOfferLanguages",
                column: "JobOfferId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateSkills_CandidateId",
                table: "CandidateSkills",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateLanguages_CandidateId",
                table: "CandidateLanguages",
                column: "CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOffers_JobTypes_JobTypeId",
                table: "JobOffers",
                column: "JobTypeId",
                principalSchema: "enum",
                principalTable: "JobTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
