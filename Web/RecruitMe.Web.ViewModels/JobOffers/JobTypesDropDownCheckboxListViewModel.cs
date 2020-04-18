namespace RecruitMe.Web.ViewModels.JobOffers
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobTypesDropDownCheckboxListViewModel : IMapFrom<JobType>, IMapTo<JobType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
