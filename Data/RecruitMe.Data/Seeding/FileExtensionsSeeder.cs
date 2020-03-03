namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class FileExtensionsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FileExtensions.Any())
            {
                return;
            }

            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Docx" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Doc" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Pdf" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Jpg" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Zip" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Eml" });
            await dbContext.FileExtensions.AddAsync(new FileExtension { Name = "Msg" });
        }
    }
}
