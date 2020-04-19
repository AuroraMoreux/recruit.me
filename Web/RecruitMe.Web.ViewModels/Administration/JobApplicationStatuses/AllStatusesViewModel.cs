namespace RecruitMe.Web.ViewModels.Administration.JobApplicationStatuses
{
    using System.Collections.Generic;

    public class AllStatusesViewModel : PaginatedModel
    {
        public IEnumerable<JobApplicationStatusViewModel> Statuses { get; set; }
    }
}
