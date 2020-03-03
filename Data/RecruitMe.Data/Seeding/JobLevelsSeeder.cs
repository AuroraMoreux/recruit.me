namespace RecruitMe.Data.Seeding
{
    using System;
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

            await dbContext.JobLevels.AddAsync(new JobLevel { Name = "Management" });
            await dbContext.JobLevels.AddAsync(new JobLevel { Name = "Expert" });
            await dbContext.JobLevels.AddAsync(new JobLevel { Name = "Entry Level" });
        }
    }
}
