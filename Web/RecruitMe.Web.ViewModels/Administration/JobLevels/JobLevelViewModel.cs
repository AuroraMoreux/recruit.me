namespace RecruitMe.Web.ViewModels.Administration.JobLevels
{
    using System;

    using RecruitMe.Data.Models.EnumModels;
    using RecruitMe.Services.Mapping;

    public class JobLevelViewModel : IMapFrom<JobLevel>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
