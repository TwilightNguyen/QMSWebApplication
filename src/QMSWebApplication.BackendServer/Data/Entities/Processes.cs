using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Process")]
    public class Processes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("Name", TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column("AreaId")]
        public int? AreaId { get; set; }
    }
}
