namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public class ApplicationDbContextSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            ILogger logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger(typeof(ApplicationDbContextSeeder));

            List<ISeeder> seeders = new List<ISeeder>
            {
                new AdminUserSeeder(),
                new DocumentCategoriesSeeder(),
                new FileExtensionsSeeder(),
                new JobApplicationStatusesSeeder(),
                new JobLevelsSeeder(),
                new JobSectorsSeeder(),
                new JobTypesSeeder(),
                new LanguagesSeeder(),
                new RolesSeeder(),
                new SettingsSeeder(),
                new SkillsSeeder(),
            };

            foreach (ISeeder seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, serviceProvider);
                await dbContext.SaveChangesAsync();
                logger.LogInformation($"Seeder {seeder.GetType().Name} done.");
            }
        }
    }
}
