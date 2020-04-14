namespace RecruitMe.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using RecruitMe.Data.Common.Repositories;
    using RecruitMe.Data.Models;
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;
    using RecruitMe.Services.Messaging;
    using RecruitMe.Web.ViewModels.JobApplications;

    public class JobApplicationService : IJobApplicationService
    {
        private readonly IDeletableEntityRepository<JobApplication> jobApplicationRepository;
        private readonly IDeletableEntityRepository<JobApplicationDocument> jobApplicationDocumentsRepository;
        private readonly IDeletableEntityRepository<JobOffer> jobOfferRepository;
        private readonly IDeletableEntityRepository<JobApplicationStatus> applicationStatusRepository;
        private readonly IEmailSender emailSender;
        private readonly IConfiguration configuration;

        public JobApplicationService(IDeletableEntityRepository<JobApplication> jobApplicationRepository, IDeletableEntityRepository<JobApplicationDocument> jobApplicationDocumentsRepository, IDeletableEntityRepository<JobOffer> jobOfferRepository, IDeletableEntityRepository<JobApplicationStatus> applicationStatusRepository, IEmailSender emailSender, IConfiguration configuration)
        {
            this.jobApplicationRepository = jobApplicationRepository;
            this.jobApplicationDocumentsRepository = jobApplicationDocumentsRepository;
            this.jobOfferRepository = jobOfferRepository;
            this.applicationStatusRepository = applicationStatusRepository;
            this.emailSender = emailSender;
            this.configuration = configuration;
        }

        public async Task<string> Apply(ApplyViewModel input, string jobOfferBaseUrl)
        {
            JobApplication jobApplication = AutoMapperConfig.MapperInstance.Map<JobApplication>(input);

            int applicationStatusId = this.applicationStatusRepository
                 .AllAsNoTracking()
                 .Where(jas => jas.Name == "Under Review")
                 .Select(jas => jas.Id)
                 .FirstOrDefault();

            jobApplication.ApplicationStatusId = applicationStatusId;
            jobApplication.CreatedOn = DateTime.UtcNow;
            await this.jobApplicationRepository.AddAsync(jobApplication);
            await this.jobApplicationRepository.SaveChangesAsync();

            List<JobApplicationDocument> jobApplicationDocuments = new List<JobApplicationDocument>();
            foreach (string documentId in input.DocumentIds)
            {
                JobApplicationDocument jobApplicationDocument = new JobApplicationDocument
                {
                    JobApplication = jobApplication,
                    DocumentId = documentId,
                };
                jobApplicationDocuments.Add(jobApplicationDocument);
            }

            await this.jobApplicationDocumentsRepository.AddRangeAsync(jobApplicationDocuments);
            await this.jobApplicationDocumentsRepository.SaveChangesAsync();

            var jobOfferDetails = this.jobOfferRepository
                .AllAsNoTracking()
                .Where(jo => jo.Id == input.JobOfferId)
                .Select(jo => new
                {
                    jo.Title,
                    jo.Employer.ContactPersonEmail,
                    jo.Employer.ContactPersonNames,
                })
                .FirstOrDefault();

            // TODO: define global email templates
            StringBuilder sb = new StringBuilder();
            sb.Append($"Hello, {jobOfferDetails.ContactPersonNames}, <br /> A new job application has been received for the following job offer: <strong>{jobOfferDetails.Title}</strong><br /><br /><strong>Candidate contact details:</strong><br /><strong>Name:</strong> {input.CandidateDetails.FirstName + " " + input.CandidateDetails.LastName}<br /><strong>Email:</strong> {input.CandidateDetails.ApplicationUserEmail}");
            if (input.CandidateDetails.PhoneNumber != null)
            {
                sb.Append($"<br /><strong>Phone Number:</strong> {input.CandidateDetails.PhoneNumber}");
            }

            sb.Append($"<br /> You can review the application details <a href='{HtmlEncoder.Default.Encode(jobOfferBaseUrl + jobApplication.Id)}'>here</a>");

            await this.emailSender.SendEmailAsync(this.configuration["DefaultAdminCredentials:Email"], this.configuration["DefaultAdminCredentials:Username"], jobOfferDetails.ContactPersonEmail, $"New Job Application received for Job Offer {jobOfferDetails.Title}", sb.ToString());

            return jobApplication.Id;
        }

        public async Task<int> ChangeJobApplicationStatus(string jobApplicationId, int statusId)
        {
            var jobApplication = this.jobApplicationRepository
                .All()
                .Where(ja => ja.Id == jobApplicationId)
                .FirstOrDefault();

            var statusExists = this.applicationStatusRepository
                .AllAsNoTracking()
                .Any(jas => jas.Id == statusId);

            if (statusExists)
            {
                jobApplication.ApplicationStatusId = statusId;
                this.jobApplicationRepository.Update(jobApplication);
                await this.jobApplicationRepository.SaveChangesAsync();
            }

            return jobApplication.ApplicationStatusId;
        }

        public T GetJobApplicationDetails<T>(string jobApplicationId)
        {
            var applicationDetails = this.jobApplicationRepository
                 .AllAsNoTracking()
                 .Where(ja => ja.Id == jobApplicationId)
                 .To<T>()
                 .FirstOrDefault();

            return applicationDetails;
        }

        public int GetJobApplicationStatusId(string jobApplicationId)
        {
            return this.jobApplicationRepository
                 .AllAsNoTracking()
                 .Where(ja => ja.Id == jobApplicationId)
                 .Select(ja => ja.ApplicationStatusId)
                 .FirstOrDefault();
        }

        public string GetJobOfferIdForApplication(string jobApplicationId)
        {
            var applicationJobOfferId = this.jobApplicationRepository
                 .AllAsNoTracking()
                 .Where(ja => ja.Id == jobApplicationId)
                 .Select(ja => ja.JobOfferId)
                 .FirstOrDefault();

            return applicationJobOfferId;
        }

        public bool HasCandidateAppliedForOffer(string candidateId, string jobOfferId)
        {
            return this.jobApplicationRepository
                 .AllAsNoTracking()
                 .Any(ja => ja.CandidateId == candidateId
                   && ja.JobOfferId == jobOfferId);
        }

        public bool IsUserRelatedToJobApplication(string jobApplicationId, string userId)
        {
            return this.jobApplicationRepository
                 .AllAsNoTracking()
                 .Any(ja => ja.Id == jobApplicationId
                  && (ja.Candidate.ApplicationUserId == userId
                  || ja.JobOffer.Employer.ApplicationUserId == userId));
        }
    }
}
