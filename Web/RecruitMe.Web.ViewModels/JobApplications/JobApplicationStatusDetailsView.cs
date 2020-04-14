namespace RecruitMe.Web.ViewModels.JobApplications
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobApplicationStatusDetailsView : IMapFrom<JobApplicationStatus>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
