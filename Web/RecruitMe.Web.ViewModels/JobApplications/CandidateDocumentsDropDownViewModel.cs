namespace RecruitMe.Web.ViewModels.JobApplications
{
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class CandidateDocumentsDropDownViewModel : IMapFrom<Document>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
