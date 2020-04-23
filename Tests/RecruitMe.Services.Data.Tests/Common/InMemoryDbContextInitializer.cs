namespace RecruitMe.Services.Data.Tests.Common
{
    using Microsoft.EntityFrameworkCore;
    using RecruitMe.Data;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class InMemoryDbContextInitializer
    {
        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            return new ApplicationDbContext(options);
        }
    }
}
