using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Shift")]
    public class Shifts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column("StartTime", TypeName = "time")]
        public TimeSpan? StartTime { get; set; }

        [Column("EndTime", TypeName = "time")]
        public TimeSpan? EndTime { get; set; }
    }
}
