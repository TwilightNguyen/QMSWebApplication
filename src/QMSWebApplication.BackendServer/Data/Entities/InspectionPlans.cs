using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("InspectionPlans")]
    public class InspectionPlans
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column("AreaId")]
        public int? AreaId { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? UploadedDateTime { get; set; }

        [Column("ModifiedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? ModifiedDateTime { get; set; }

        [Column("Version")]
        public int? Version { get; set; }

        [Column("Enabled")]
        public bool? Enabled { get; set; }
    }
}
