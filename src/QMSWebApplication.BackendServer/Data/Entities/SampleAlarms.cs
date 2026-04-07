using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("SampleAlarms")]
    public class SampleAlarms
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [Column("AlarmValue")]
        public int? AlarmValue { get; set; }

        [MaxLength(50)]
        [Column("Description", TypeName = "nvarchar(50)")]
        public string? Description { get; set; }
    }
}
