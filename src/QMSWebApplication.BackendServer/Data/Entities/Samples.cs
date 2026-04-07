using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Samples")]
    public class Samples
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CharacteristicId")]
        public int? CharacteristicId { get; set; }

        [MaxLength(64)]
        [Column("CharacteristicValue", TypeName = "nvarchar(64)")]
        public string? CharacteristicValue { get; set; }

        [MaxLength(64)]
        [Column("CharacteristicRange", TypeName = "nvarchar(64)")]
        public string? CharacteristicRange { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? UploadedDateTime { get; set; }

        [Column("DataCollection")]
        public int? DataCollection { get; set; }

        [Column("MeasuredDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? MeasuredDateTime { get; set; }

        [Column("JobId")]
        public int? JobId { get; set; }

        [Column("ProductionId")]
        public int? ProductionId { get; set; }

        [Column("LineId")]
        public int? LineId { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; }

        [Column("OutputQuantity")]
        public int? OutputQuantity { get; set; }

        [Column("SampleIndex")]
        public int? SampleIndex { get; set; }

        [Column("Status")]
        public int? Status { get; set; }

        [Column("EmailSent")]
        public int? EmailSent { get; set; }

        [Column("PlanTypeId")]
        public int? PlanTypeId { get; set; }

        [MaxLength(50)]
        [Column("Notes", TypeName = "nvarchar(50)")]
        public string? Notes { get; set; }

        [Column("SampleQuantity")]
        public int? SampleQuantity { get; set; }

        [Column("MoldId")]
        public int? MoldId { get; set; }

        [Column("CavityId")]
        public int? CavityId { get; set; } 
    }
}
