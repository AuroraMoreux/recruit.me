namespace RecruitMe.Web.ViewModels.Documents
{
    using System.Collections.Generic;

    public class CandidateDocumentsViewModel : PaginatedModel
    {
        public IEnumerable<DocumentsViewModel> Documents { get; set; }
    }
}
