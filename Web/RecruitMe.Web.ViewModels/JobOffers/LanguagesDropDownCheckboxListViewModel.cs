namespace RecruitMe.Web.ViewModels.JobOffers
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class LanguagesDropDownCheckboxListViewModel : IMapFrom<Language>, IMapTo<Language>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
