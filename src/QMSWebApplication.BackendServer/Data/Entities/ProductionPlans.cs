using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("ProductionData")]
    public class ProductionPlans
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("LineId")]
        public int? LineId { get; set; }

        [Column("StartTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? StartTime { get; set; }

        [Column("EndTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? EndTime { get; set; }

        [Column("JobId")]
        public int? JobId { get; set; }

        [Column("ProductionDate", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? ProductionDate { get; set; }

        [Column("PlannedQuantity")]
        public int? PlannedQuantity { get; set; }

        [MaxLength(100)]
        [Column("LotInform", TypeName = "nvarchar(100)")]
        public string? LotInform { get; set; }

        [MaxLength(50)]
        [Column("MaterialInform", TypeName = "nvarchar(50)")]
        public string? MaterialInform { get; set; }

        [Column("CNCLatheMachine")]
        public int? CNCLatheMachine { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [MaxLength(100)]
        [Column("Notes", TypeName = "nvarchar(100)")]
        public string? Notes { get; set; }

        [Column("Ennabled")]
        public bool? Enabled { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset UploadedDateTime { get; set; }

        [Column("ModifiedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? ModifiedDateTime { get; set; }
    }
}
