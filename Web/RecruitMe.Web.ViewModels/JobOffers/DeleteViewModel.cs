namespace RecruitMe.Web.ViewModels.JobOffers
{
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DeleteViewModel : IMapFrom<JobOffer>
    {
        public string Id { get; set; }

        public string Position { get; set; }
    }
}
