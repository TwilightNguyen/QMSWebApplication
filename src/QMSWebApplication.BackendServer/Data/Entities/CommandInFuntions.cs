using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("CommandInFuntions")]
    [PrimaryKey(nameof(CommandId), nameof(FunctionId))]
    public class CommandInFuntions
    {
        [Column("CommandId")]
        public required int CommandId { get; set; }

        [Column("FunctionId")]
        public required int FunctionId { get; set; }
    }
}
