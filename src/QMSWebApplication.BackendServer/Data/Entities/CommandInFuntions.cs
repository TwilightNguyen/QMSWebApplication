using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("CommandInFuntions")]
    public class CommandInFuntions
    {
        [Key]
        [Column("CommandId")]
        public required int CommandId { get; set; }

        [Key]
        [Column("FunctionId")]
        public required int FunctionId { get; set; }
    }
}
