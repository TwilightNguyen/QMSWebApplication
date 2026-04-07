using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("InspectionPlanData")]
    public class InspectionPlanData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("InspPlanSubId")]
        public int? InspPlanSubId { get; set; }

        [Column("CharacteristicId")]
        public int? CharacteristicId { get; set; }

        [Column("LSL")]
        public double? LSL { get; set; }

        [Column("USL")]
        public double? USL { get; set; }

        [Column("LCL")]
        public double? LCL { get; set; }

        [Column("UCL")]
        public double? UCL { get; set; }

        [Column("EnabledSPCChart")]
        public bool? EnabledSPCChart { get; set; }

        [Column("DataEntry")]
        public bool? DataEntry { get; set; }

        [Column("CpkMax")]
        public double? CpkMax { get; set; }

        [Column("CpkMin")]
        public double? CpkMin { get; set; }

        [Column("EnabledCpkControl")]
        public bool? EnabledCpkControl { get; set; }

        [MaxLength(20)]
        [Column("Notes", TypeName = "nvarchar(20)")]
        public string? Notes { get; set; }

        [Column("PercentControlLimit")]
        public double? PercentControlLimit { get; set; }

        [Column("Enabled")]
        public bool? Enabled { get; set; }
    }
}
