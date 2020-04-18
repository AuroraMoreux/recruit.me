namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
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

            List<string> types = new List<string> { "Permanent", "Replacement", "Seasonal", "Internship", "Remote", "Full-Time" };

            foreach (string type in types)
            {
                await dbContext.JobTypes.AddAsync(new JobType
                {
                    Name = type,
                });
            }
        }
    }
}
