using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("EventLogs")]
    public class EventLogs
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("EventTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset EventTime { get; set; }

        [MaxLength(10)]
        [Column("EventCode", TypeName = "nvarchar(10)")]
        public string? EventCode { get; set; }

        [MaxLength(200)]
        [Column("Description", TypeName = "nvarchar(200)")]
        public string? Description { get; set; }

        [MaxLength(30)]
        [Column("Station", TypeName = "nvarchar(30)")]
        public string? Station { get; set; }
    }
}
