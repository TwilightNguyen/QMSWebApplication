using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("WebSession")]
    public class WebSession
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("SessionId", TypeName = "nvarchar(100)")]
        public string? SessionId { get; set; }

        [MaxLength(50)]
        [Column("IpAddress", TypeName = "nvarchar(50)")]
        public string? IpAddress { get; set; }

        [Column("StartTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? StartTime { get; set; }

        [Column("EndTime", TypeName = "DateTimeOffset(3)")]
        public DateTimeOffset? EndTime { get; set; }
    }
}
