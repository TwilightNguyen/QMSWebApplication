using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("EventEmailHistories")]
    public class EventEmailHistories 
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("CharacteristicId")]
        public int? CharacteristicId { get; set; }

        [Column("ProductionId")]
        public int? ProductionId { get; set; }

        [Column("DefectRate")]
        public int? DefectRate { get; set; }

        [Column("AEActive")]
        public int? AEActive { get; set; }

        [Column("TimeIn", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? TimeIn { get; set; }

        [Column("EmailAddress", TypeName = "nvarchar(max)")]
        public string? EmailAddress { get; set; }

    }
}
