namespace RecruitMe.Web.ViewModels.Administration.FileExtensions
{
    using System.Collections.Generic;

    public class AllFileExtensionsViewModel : PaginatedModel
    {
        public IEnumerable<ExtensionsViewModel> Extensions { get; set; }
    }
}
