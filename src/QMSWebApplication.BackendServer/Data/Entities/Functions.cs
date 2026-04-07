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
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        [Column("Url", TypeName = "nvarchar(100)")]
        public required string Url { get; set; }

        [Column("OrderNumber")]
        public int? OrderNumber { get; set; }

        [Column("ParentId")]
        public int? ParentId { get; set; }

        [Column("UploadedDateTime")]
        public DateTimeOffset? UploadedDateTime { get; set; }

        [Column("ModifiedDateTime")]
        public DateTimeOffset? ModifiedDateTime { get; set; }

        [Column("Enabled")]
        public bool Enabled { get; set; } = true;
    }
}
