namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class JobTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.JobTypes.Any())
            {
                return;
            }

            await dbContext.JobTypes.AddAsync(new JobType { Name = "Permanent" });
            await dbContext.JobTypes.AddAsync(new JobType { Name = "Replacement" });
            await dbContext.JobTypes.AddAsync(new JobType { Name = "Seasonal" });
            await dbContext.JobTypes.AddAsync(new JobType { Name = "Internship" });
        }
    }
}
