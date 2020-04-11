namespace RecruitMe.Web.ViewModels.JobOffers
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class SkillsDropDownCheckboxListViewModel : IMapFrom<Skill>, IMapTo<Skill>
    {
        // TODO: Split in partial view
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
