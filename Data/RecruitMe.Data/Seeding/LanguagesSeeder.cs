namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using RecruitMe.Data.Models.EnumModels;

    internal class LanguagesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Languages.Any())
            {
                return;
            }

            var languages = new List<string> { "Bulgarian", "Croatian", "Czech", "Danish", "English", "Estonian", "Finnish", "French", "German", "Greek", "Hungarian", "Irish", "Italian", "Latvian", "Lithuanian", "Maltese", "Polish", "Portuguese", "Romanian", "Slovak", "Slovene", "Spanish", "Swedish" };

            foreach (var laguage in languages)
            {
                await dbContext.Languages.AddAsync(new Language
                {
                    Name = laguage,
                });
            }
        }
    }
}
