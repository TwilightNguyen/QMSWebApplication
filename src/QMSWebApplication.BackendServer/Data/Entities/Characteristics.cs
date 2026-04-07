using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Characteristics")]
    public class Characteristics 
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        [Column("Name", TypeName = "nvarchar(200)")]
        public required string Name { get; set; }

        [Column("MeaTypeId")]
        public int? MeaTypeId { get; set; }

        [Column("ProcessId")]
        public required int ProcessId { get; set; }

        [Column("DataType")]
        public int? DataType { get; set; }

        [MaxLength(10)]
        [Column("Unit", TypeName = "nvarchar(10)")]
        public string? Unit { get; set; }

        [Column("Enabled")]
        public bool? Enabled { get; set; } = false;

        [Column("DefectRateLimit")]
        public int? DefectRateLimit { get; set; }

        [Column("EmailEventModel")]
        public int? EmailEventModel { get; set; }

        [Column("Decimals")]
        public int? Decimals { get; set; }
    }
}
    