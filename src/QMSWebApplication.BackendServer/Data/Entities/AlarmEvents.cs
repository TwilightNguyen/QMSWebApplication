using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("AlarmEvents")]
    public class AlarmEvents
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        [Column("Description", TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        [Column("Active")]
        public int? Active { get; set; }

        [MaxLength(50)]
        [Column("RoleId", TypeName = "nvarchar(50)")]
        public string? RoleId { get; set; }

        [Column("LineId")]
        public int? LineId { get; set; }

        [Column("Enable")]
        public int? Enable { get; set; }

        [Column("EventSent")]
        public int? EventSent { get; set; }
    }
}
