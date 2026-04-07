using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("EventEmailDatas")]
    public class EventEmailDatas
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("EventRoleId")]
        public int? EventRoleId { get; set; }

        [Column("ProductionId")]
        public int? ProductionId { get; set; }

        [Column("DefectRate")]
        public int? DefectRate { get; set; }

        [Column("AEActive")]
        public int? AEActive { get; set; }

        [Column("EventSent")]
        public int? EventSent { get; set; }

        [Column("EventSentFail")]
        public int? EventSentFail { get; set; }

        [Column("ModifiedDatetime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? ModifiedDatetime { get; set; }

        [Column("OutSpec")]
        public int? OutSpec { get; set; }

        [Column("Quantity")]
        public int? Quantity { get; set; }

        [Column("EmailSentTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? EmailSentTime { get; set; }

    }
}
