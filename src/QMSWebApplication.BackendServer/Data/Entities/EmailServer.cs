using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("EmailServer")]
    public class EmailServer
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        [Column("EmailAddress", TypeName = "nvarchar(100)")]
        public string? EmailAddress { get; set; }

        [MaxLength(100)]
        [Column("DisplayName", TypeName = "nvarchar(100)")]
        public string? DisplayName { get; set; }

        [MaxLength(100)]
        [Column("Password", TypeName = "nvarchar(100)")]
        public string? Password { get; set; }

        [MaxLength(50)]
        [Column("SMTPHost", TypeName = "nvarchar(50)")]
        public string? SMTPHost { get; set; }

        [Column("SMTPPort")]
        public int? SMTPPort { get; set; }

        [Column("EnabledSSL")]
        public bool? EnabledSSL { get; set; }
    }
}
