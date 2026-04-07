using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("TVDisplay")]
    public class TVDisplays
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(50)]
        [Column("Name", TypeName = "nvarchar(50)")]
        public string? Name { get; set; }
    }
}
