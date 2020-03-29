namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("DocumentCategories", Schema = "enum")]

    public class DocumentCategory : BaseModel<int>
    {
        public DocumentCategory()
        {
            this.Documents = new HashSet<Document>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
