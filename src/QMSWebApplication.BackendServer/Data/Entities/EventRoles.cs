using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("EventRoles")]
    public class EventRoles
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Column("CharacteristicId")]
        public int? CharacteristicId { get; set; }

        [MaxLength(50)]
        [Column("RoleId", TypeName = "nvarchar(50)")]
        public string? RoleId { get; set; }
    }
}
