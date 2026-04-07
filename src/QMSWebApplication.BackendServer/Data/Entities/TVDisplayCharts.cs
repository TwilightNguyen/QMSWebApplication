using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    public class TVDisplayCharts
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("TVDisplayId")]
        public int TvDisplayId { get; set; }

        [MaxLength(50)]
        [Column("ProductionId")]
        public required int ProductionId { get; set; }

        [MaxLength(50)]
        [Column("CharacteristicID")]
        public string? CharacteristicId { get; set; }

        [MaxLength(50)]
        [Column("PlanTypeId")]
        public string? PlanTypeId { get; set; }

        [MaxLength(50)]
        [Column("MoldId")]
        public string? MoldId { get; set; }

        [MaxLength(50)]
        [Column("CavityId")]
        public string? CavityId { get; set; }

        [Column("DisplayOrder")]
        public int? DisplayOrder { get; set; }
    }
}
