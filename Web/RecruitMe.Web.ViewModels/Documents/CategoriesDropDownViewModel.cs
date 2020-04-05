namespace RecruitMe.Web.ViewModels.Documents
{
    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class CategoriesDropDownViewModel : IMapFrom<DocumentCategory>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
