namespace RecruitMe.Web.ViewModels.Administration.JobTypes
{
    using System.Collections.Generic;

    public class AllJobTypesViewModel : PaginatedModel
    {
        public IEnumerable<JobTypesViewModel> Types { get; set; }
    }
}
