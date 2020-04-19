namespace RecruitMe.Web.ViewModels.Administration.JobSectors
{
    using System.Collections.Generic;

    public class AllJobSectorsViewModel : PaginatedModel
    {
        public IEnumerable<JobSectorsViewModel> Sectors { get; set; }
    }
}
