namespace RecruitMe.Web.ViewModels.Employers
{

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobSectorsDropDownViewModel : IMapFrom<JobSector>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
