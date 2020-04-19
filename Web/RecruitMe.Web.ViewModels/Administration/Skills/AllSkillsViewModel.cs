namespace RecruitMe.Web.ViewModels.Administration.Skills
{
    using System.Collections.Generic;

    public class AllSkillsViewModel : PaginatedModel
    {
        public IEnumerable<SkillsViewModel> Skills { get; set; }
    }
}
