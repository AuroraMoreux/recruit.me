namespace RecruitMe.Data.Seeding
{
    using System;
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

            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Open" });
            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Under Review" });
            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Accepted" });
            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Rejected" });
            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Retracted" });
            await dbContext.ApplicationStatuses.AddAsync(new JobApplicationStatus { Name = "Closed" });
        }
    }
}
