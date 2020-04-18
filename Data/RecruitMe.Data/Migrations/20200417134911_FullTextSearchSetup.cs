namespace RecruitMe.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class FullTextSearchSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: @"IF (FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1)
                       IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE [name] = 'FTCatalog')
                    PRINT 'FullText Catalog FTCatalog lready exists'
                     ELSE CREATE FULLTEXT CATALOG FTCatalog WITH ACCENT_SENSITIVITY = OFF AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: @"IF (FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1)
                       IF (COLUMNPROPERTY(OBJECT_ID('JobOffers'), 'Description', 'IsFulltextIndexed') = 1)
                    PRINT 'FullText Index ON dbo.JobOffers(Description) already exists'
                     ELSE CREATE FULLTEXT INDEX ON dbo.JobOffers(Description) KEY INDEX [PK_JobOffers];",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
