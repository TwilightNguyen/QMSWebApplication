using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Permissions")]
    public class Permissions
    {

        [Key]
        [Column("FunciontId")]
        public required int FunciontId { get; set; }

        [Key]
        [Column("RoleId")]
        public required int RoleId { get; set; }

        [Key]
        [Column("CommandId")]
        public required int CommandId { get; set; }
    }
}
