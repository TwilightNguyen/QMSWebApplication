using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("JobDecisions")]
    public class JobDecisions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(20)]
        [Column("Decision", TypeName = "nvarchar(20)")]
        public string? Decision { get; set; }

        [MaxLength(20)]
        [Column("ColorCode", TypeName = "nvarchar(20)")]
        public string? ColorCode { get; set; }
    }
}
