namespace RecruitMe.Web.ViewModels.Documents
{
    using System.Collections.Generic;

    public class UserDocumentsViewModel
    {
        public IEnumerable<DocumentsViewModel> Documents { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }
    }
}
