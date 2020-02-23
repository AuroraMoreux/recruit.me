namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models;

    internal class JobSectorsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.JobSectors.Any())
            {
                return;
            }

            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Accountancy" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Administration" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Advertising" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Aerospace" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Automotive" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Banking" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Catering" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Charity" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Construction" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "ConsumerGoods" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Creative" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "CustomerService" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Digital" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Education" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Energy" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Engineering" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Entertainment" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Finance" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Graduate" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Healthcare" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Hospitality" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "HR" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Insurance" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "IT" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Legal" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Leisure" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Logistics" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "ManagementConsultancy" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Manufacturing" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Marketing" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Media" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Nursing" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Pharmaceutical" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "PR" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "PublicSector" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "RealEstate" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Retail" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Sales" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Science" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "SocialWork" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Support" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Telecoms" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Tourism" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Transport" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Travel" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Utilities" });
            await dbContext.JobSectors.AddAsync(new JobSector { Name = "Wholesale" });
        }
    }
}
