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
        private readonly RecruitMe.Data.Common.Repositories.IDeletableEntityRepository<Employer> employersRepository;
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
                string logoUrl = await CloudinaryService.UploadImageAsync(this.cloudinary, model.Logo, model.ApplicationUserId + "_Logo");

                employer.LogoUrl = logoUrl;
            }

            await this.employersRepository.AddAsync(employer);
            await this.employersRepository.SaveChangesAsync();

            return employer.Id;
        }

        public T GetProfileDetailsAsync<T>(string employerId)
        {
            T employer = this.employersRepository
                 .All()
                 .Where(e => e.Id == employerId)
                 .To<T>()
                 .FirstOrDefault();

            return employer;
        }
    }
}
