using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Permissions")]
    [PrimaryKey(nameof(CommandId), nameof(FunctionId), nameof(RoleId))]
    public class Permissions
    {

        [Key]
        [Column("FunctionId")]
        public required int FunctionId { get; set; }

        [Key]
        [Column("RoleId")]
        public required int RoleId { get; set; }

        [Key]
        [Column("CommandId")]
        public required int CommandId { get; set; }
    }
}
