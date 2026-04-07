using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Commands")]
    public class Commands
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }

        [Column("Notes", TypeName = "nvarchar(255)")]
        public string? Notes { get; set; }

        [Column("UploadedDateTime")]
        public DateTimeOffset? UploadedDateTime { get; set; } = DateTimeOffset.Now;

        [Column("ModifiedDateTime")]
        public DateTimeOffset? ModifiedDateTime { get; set; }

        [Column("DisplayOrder")]
        public int DisplayOrder { get; set; }

        [Column("Enabled")]
        public bool Enabled { get; set; }
    }
}
