namespace RecruitMe.Web.ViewModels.Documents
{
    using RecruitMe.Data.Models;
    using RecruitMe.Services.Mapping;

    public class DeleteViewModel : IMapFrom<Document>
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
