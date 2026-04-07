using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("InspectionPlanTracking")]
    public class InspectionPlanTracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("InspPlanId")]
        public int? InspPlanId { get; set; }

        [Column("CharacteristicId")]
        public int? CharacteristicId { get; set; }

        [Column("PlanState")]
        public int? PlanState { get; set; }

        [Column("PlanTypeId")]
        public int? PlanTypeId { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? UploadedDateTime { get; set; }

        [Column("UserId")]
        public int? UserId { get; set; } 
    }
}
