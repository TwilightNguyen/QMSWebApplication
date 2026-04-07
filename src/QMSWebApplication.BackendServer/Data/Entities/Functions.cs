using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Functions")]
    public class Functions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public required int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        [Column("Url", TypeName = "nvarchar(100)")]
        public required string Url { get; set; }

        [Column("SortOrder")]
        public int? SortOrder { get; set; }

        [Column("ParentId")]
        public int? ParentId { get; set; }
    }
}
