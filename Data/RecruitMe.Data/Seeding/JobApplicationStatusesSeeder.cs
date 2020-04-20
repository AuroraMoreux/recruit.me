namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    public class JobApplicationStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.ApplicationStatuses.Any())
            {
                return;
            }

            List<string> statuses = new List<string> { "Under Review", "Accepted", "Rejected", "Retracted" };

            foreach (string status in statuses)
            {
                await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus
                {
                    Name = status,
                });
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
