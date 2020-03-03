namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class DocumentCategoriesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.DocumentCategories.Any())
            {
                return;
            }

            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Resume" });
            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Cover Letter" });
            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Reference List" });
            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Letter Of Recommendation" });
            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Portflolio" });
            await dbContext.DocumentCategories.AddAsync(new DocumentCategory { Name = "Certificate" });
        }
    }
}
