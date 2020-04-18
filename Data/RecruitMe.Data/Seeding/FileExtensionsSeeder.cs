namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
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

            Dictionary<string, string> extensions = new Dictionary<string, string>
            {
                { ".docx", "File" },
                { ".doc", "File" },
                { ".pdf", "File" },
                { ".eml", "File" },
                { ".msg", "File" },
                { ".jpg", "Image" },
                { ".jpeg", "Image" },
                { ".png", "Image" },
            };

            foreach (string key in extensions.Keys)
            {
                await dbContext.FileExtensions.AddAsync(new FileExtension
                {
                    Name = key,
                    FileType = extensions[key],
                });
            }
        }
    }
}
