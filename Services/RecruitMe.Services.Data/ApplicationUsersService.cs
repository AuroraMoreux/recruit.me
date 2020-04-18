namespace RecruitMe.Services.Data
{
    using System;
    using System.Linq;

    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;

    public class ApplicationUsersService : IApplicationUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> applicationUsersRepository;

        public ApplicationUsersService(IDeletableEntityRepository<ApplicationUser> applicationUsersRepository)
        {
            this.applicationUsersRepository = applicationUsersRepository;
        }

        public int GetCount()
        {
            return this.applicationUsersRepository
                .AllAsNoTracking()
                .Count();
        }

        public int GetNewUsersCount()
        {
            DateTime yesterdaysDate = DateTime.UtcNow.AddDays(-1).Date;
            return this.applicationUsersRepository
                .AllAsNoTracking()
                .Where(au => au.CreatedOn >= yesterdaysDate)
                .Count();
        }
    }
}
