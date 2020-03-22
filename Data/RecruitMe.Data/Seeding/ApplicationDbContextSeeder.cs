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
                new RolesSeeder(),
                new AdminUserSeeder(),
                new SettingsSeeder(),
                new LanguagesSeeder(),
                new JobSectorsSeeder(),
                new JobApplicationStatusesSeeder(),
                new DocumentCategoriesSeeder(),
                new FileExtensionsSeeder(),
                new JobLevelsSeeder(),
                new JobTypesSeeder(),
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
