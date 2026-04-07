using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Commands")]
    public class Commands
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public required string Id { get; set; }

        [Column("Name", TypeName = "nvarchar(100)")]
        public required string Name { get; set; }
    }
}
