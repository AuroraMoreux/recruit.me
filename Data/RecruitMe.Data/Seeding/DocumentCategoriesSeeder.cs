namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
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

            var documentCategories = new List<string> { "Resume", "Cover Letter", "Reference List", "Letter Of Recommendation", "Portflolio", "Certificate" };

            foreach (var category in documentCategories)
            {
                await dbContext.DocumentCategories.AddAsync(new DocumentCategory
                {
                    Name = category,
                });
            }
        }
    }
}
