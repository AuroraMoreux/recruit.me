namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("DocumentCategories", Schema = "enum")]

    public class DocumentCategory
    {
        public DocumentCategory()
        {
            this.Documents = new HashSet<Document>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Document> Documents { get; set; }
    }
}
