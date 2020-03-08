namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class JobLevelsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.JobLevels.Any())
            {
                return;
            }

            List<string> levels = new List<string> { "Management", "Expert", "Entry Level" };

            foreach (string level in levels)
            {
                await dbContext.JobLevels.AddAsync(new JobLevel
                {
                    Name = level,
                });
            }
        }
    }
}
