namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Web.ViewModels.Employers;

    public class EmployersService : IEmployersService
    {
        private const string LogoNameAddIn = "_logo";
        private readonly IDeletableEntityRepository<Employer> employersRepository;
        private readonly Cloudinary cloudinary;

        public EmployersService(
            IDeletableEntityRepository<Employer> employersRepository,
            Cloudinary cloudinary)
        {
            this.employersRepository = employersRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<string> CreateProfileAsync(CreateEmployerProfileInputModel model)
        {
            var employer = AutoMapperConfig.MapperInstance.Map<Employer>(model);

            if (model.Logo != null)
            {
                var logoUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.Logo, model.ApplicationUserId + LogoNameAddIn);

                if (logoUrl == null)
                {
                    return null;
                }

                employer.LogoUrl = logoUrl;
            }

            employer.CreatedOn = DateTime.UtcNow;
            try
            {
                await this.employersRepository.AddAsync(employer);
                await this.employersRepository.SaveChangesAsync();
                return employer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetCount()
        {
            return this.employersRepository
                .AllAsNoTracking()
                .Count();
        }

        public string GetEmployerIdByUsername(string username)
        {
            return this.employersRepository
                 .AllAsNoTracking()
                 .Where(e => e.ApplicationUser.UserName == username)
                 .Select(e => e.Id)
                 .FirstOrDefault();
        }

        public string GetEmployerNameById(string id)
        {
            return this.employersRepository
                 .AllAsNoTracking()
                 .Where(e => e.Id == id)
                 .Select(e => e.Name)
                 .FirstOrDefault();
        }

        public int GetNewEmployersCount()
        {
            var yesterdaysDate = DateTime.UtcNow.AddDays(-1).Date;
            return this.employersRepository
                .AllAsNoTracking()
                .Where(e => e.ApplicationUser.CreatedOn >= yesterdaysDate)
                .Count();
        }

        public T GetProfileDetails<T>(string employerId)
        {
            return this.employersRepository
                 .AllAsNoTracking()
                 .Where(e => e.Id == employerId)
                 .To<T>()
                 .FirstOrDefault();
        }

        public IEnumerable<T> GetTopFiveEmployers<T>()
        {
            return this.employersRepository
                .AllAsNoTracking()
                .OrderByDescending(e => e.JobOffers.Count)
                .Take(5)
                .To<T>()
                .ToList();
        }

        public async Task<string> UpdateProfileAsync(string employerId, UpdateEmployerProfileViewModel model)
        {
            var employer = this.employersRepository
                  .All()
                  .FirstOrDefault(e => e.Id == employerId);

            if (employer == null)
            {
                return null;
            }

            employer.Address = model.Address;
            employer.ContactPersonEmail = model.ContactPersonEmail;
            employer.ContactPersonNames = model.ContactPersonNames;
            employer.ContactPersonPhoneNumber = model.ContactPersonPhoneNumber;
            employer.ContactPersonPosition = model.ContactPersonPosition;
            employer.IsHiringAgency = model.IsHiringAgency;
            employer.IsPublicSector = model.IsPublicSector;
            employer.JobSectorId = model.JobSectorId;
            employer.PhoneNumber = model.PhoneNumber;
            employer.Name = model.Name;
            employer.UniqueIdentificationCode = model.UniqueIdentificationCode;
            employer.WebsiteAddress = model.WebsiteAddress;

            if (model.Logo != null)
            {
                if (employer.LogoUrl != null)
                {
                    CloudinaryService.DeleteFile(this.cloudinary, model.ApplicationUserId + LogoNameAddIn);
                }

                var logoUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.Logo, model.ApplicationUserId + LogoNameAddIn);
                if (logoUrl == null)
                {
                    return null;
                }

                employer.LogoUrl = logoUrl;
            }

            employer.ModifiedOn = DateTime.UtcNow;
            try
            {
                this.employersRepository.Update(employer);
                await this.employersRepository.SaveChangesAsync();
                return employer.Id;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
