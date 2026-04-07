using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Jobs")]
    public class Jobs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("AreaId")]
        public int? AreaId { get; set; }

        [Column("ProductId")]
        public int? ProductId { get; set; }

        [MaxLength(100)]
        [Column("JobCode", TypeName = "nvarchar(100)")]
        public string? JobCode { get; set; }

        [MaxLength(100)]
        [Column("POCode", TypeName = "nvarchar(100)")]
        public string? POCode { get; set; }

        [MaxLength(100)]
        [Column("SOCode", TypeName = "nvarchar(100)")]
        public string? SOCode { get; set; }

        [Column("PlannedQuantity")]
        public int? PlannedQuantity { get; set; }

        [Column("OutputQuantity")]
        public int? OutputQuantity { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? UploadedDateTime { get; set; }

        [Column("JobDecisionId")]
        public int? JobDecisionId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("Enabled")]
        public bool? Enabled { get; set; }
    }
}
