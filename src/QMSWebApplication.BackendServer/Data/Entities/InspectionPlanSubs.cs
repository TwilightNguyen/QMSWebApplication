using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("InspectionPlanSubs")]
    public class InspectionPlanSubs
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("InspPlanId")]
        public int InspPlanId { get; set; }

        [Column("PlanTypeId")]
        public int PlanTypeId { get; set; }

        [Column("PlanState")]
        public int PlanState { get; set; }

        [Column("UploadedDateTime", TypeName = "DateTimeOffset")]
        public DateTimeOffset UploadedDateTime { get; set; } = DateTime.Now;

        [Column("Enabled")]
        public bool Enabled { get; set; } = false;
    }
}
