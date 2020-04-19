namespace RecruitMe.Web.ViewModels.Administration.DocumentCategories
{
    using System.Collections.Generic;

    public class AllDocumentCategoriesViewModel : PaginatedModel
    {
        public IEnumerable<DocumentCategoriesViewModel> DocumentCategories { get; set; }
    }
}
