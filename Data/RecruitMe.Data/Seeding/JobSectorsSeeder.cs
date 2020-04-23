namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    internal class JobSectorsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.JobSectors.Any())
            {
                return;
            }

            var sectors = new List<string> { "Accountancy", "Administration", "Advertising", "Aerospace", "Automotive", "Banking", "Catering", "Charity", "Construction", "Consumer Goods", "Creative", "Customer Service", "Digital", "Education", "Energy", "Engineering", "Entertainment", "Finance", "Graduate", "Healthcare", "Hospitality", "HR", "Insurance", "IT", "Legal", "Leisure", "Logistics", "Management and Consultancy", "Manufacturing", "Marketing", "Media", "Nursing", "Pharmaceutical", "PR", "Public Sector", "Real Estate", "Retail", "Sales", "Science", "Social Work", "Support", "Telecoms", "Tourism", "Transport", "Travel", "Utilities", "Wholesale" };

            foreach (var sector in sectors)
            {
                await dbContext.JobSectors.AddAsync(new JobSector
                {
                    Name = sector,
                });
            }
        }
    }
}
