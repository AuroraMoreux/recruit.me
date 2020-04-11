namespace RecruitMe.Web.ViewModels.JobOffers
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobLevelsCheckboxViewModel : IMapFrom<JobLevel>, IMapTo<JobLevel>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
