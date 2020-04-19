namespace RecruitMe.Web.ViewModels.Administration.Languages
{
    using System.Collections.Generic;

    public class AllLanguagesViewModel : PaginatedModel
    {
        public IEnumerable<LanguagesViewModel> Languages { get; set; }
    }
}
