using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("ProcessLine")]
    public class ProcessLines
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [MaxLength(100)]
        [Column("LineCode", TypeName = "nvarchar(100)")]
        public string? LineCode { get; set; }

        [Column("ProcessId")]
        public int? ProcessId { get; set; }
    }
}
