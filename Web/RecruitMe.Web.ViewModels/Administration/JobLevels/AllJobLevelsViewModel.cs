namespace RecruitMe.Web.ViewModels.Administration.JobLevels
{
    using System.Collections.Generic;

    public class AllJobLevelsViewModel : PaginatedModel
    {
        public IEnumerable<JobLevelViewModel> Levels { get; set; }
    }
}
