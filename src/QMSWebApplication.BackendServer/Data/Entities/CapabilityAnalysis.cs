using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("CapabilityAnalysis")]
    public class CapabilityAnalysis
    {
        [Key]
        [Column("JobId", Order = 0)]
        public int JobId { get; set; }

        [Key]
        [Column("PlanTypeId", Order = 1)]
        public int PlanTypeId { get; set; }

        [Key]
        [Column("CharacteristicId", Order = 2)]
        public int CharacteristicId { get; set; }

        [Column("ProductedQty")]
        public int? ProductedQty { get; set; }

        [Column("Average")]
        public float? Average { get; set; }

        [Column("StDevWithin")]
        public float? StDevWithin { get; set; }

        [Column("StDevOverall")]
        public float? StDevOverall { get; set; }
    }
}
