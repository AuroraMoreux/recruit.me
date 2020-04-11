namespace RecruitMe.Services.Data
{
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

        public EmployersService(IDeletableEntityRepository<Employer> employersRepository, Cloudinary cloudinary)
        {
            this.employersRepository = employersRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<string> CreateProfileAsync(CreateEmployerProfileInputModel model)
        {
            Employer employer = AutoMapperConfig.MapperInstance.Map<Employer>(model);

            if (model.Logo != null)
            {
                string logoUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.Logo, model.ApplicationUserId + LogoNameAddIn);

                employer.LogoUrl = logoUrl;
            }

            await this.employersRepository.AddAsync(employer);
            await this.employersRepository.SaveChangesAsync();

            return employer.Id;
        }

        public string GetEmployerIdByUsername(string username)
        {
            return this.employersRepository
                 .All()
                 .Where(e => e.ApplicationUser.UserName == username)
                 .Select(e => e.Id)
                 .FirstOrDefault();
        }

        public T GetProfileDetails<T>(string employerId)
        {
            T employer = this.employersRepository
                 .All()
                 .Where(e => e.Id == employerId)
                 .To<T>()
                 .FirstOrDefault();

            return employer;
        }

        public async Task<string> UpdateProfileAsync(string employerId, UpdateEmployerProfileViewModel model)
        {
            Employer employer = this.employersRepository
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

            if (employer.LogoUrl != null)
            {
                CloudinaryService.DeleteFile(this.cloudinary, model.ApplicationUserId + LogoNameAddIn);
                employer.LogoUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.Logo, model.ApplicationUserId + LogoNameAddIn);
            }

            this.employersRepository.Update(employer);

            await this.employersRepository.SaveChangesAsync();

            return employer.Id;
        }
    }
}
