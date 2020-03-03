namespace RecruitMe.Data.Models.EnumModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using RecruitMe.Data.Common.Models;

    [Table("FileExtensions", Schema = "enum")]

    public class FileExtension : BaseDeletableModel<int>
    {
        public FileExtension()
        {
            this.Documents = new HashSet<Document>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}
