﻿namespace RecruitMe.Data.Seeding
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

            List<string> extensions = new List<string> { ".docx", ".doc", ".pdf", ".jpg", ".jpeg", ".png", ".eml", ".msg" };

            foreach (string extension in extensions)
            {
                await dbContext.FileExtensions.AddAsync(new FileExtension
                {
                    Name = extension,
                });
            }
        }
    }
}
