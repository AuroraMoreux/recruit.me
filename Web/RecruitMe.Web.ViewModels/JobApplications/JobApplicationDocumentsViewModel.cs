namespace RecruitMe.Web.ViewModels.JobApplications
{
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class JobApplicationDocumentsViewModel : IMapFrom<JobApplicationDocument>
    {
        public string DocumentId { get; set; }

        public string DocumentName { get; set; }

        public string DocumentDocumentCategoryName { get; set; }
    }
}
