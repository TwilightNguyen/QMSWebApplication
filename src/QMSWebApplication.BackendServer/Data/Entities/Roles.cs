using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    public class Roles : IdentityRole<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("intRoleID")]
        public int IntRoleID { get; set; }

        [MaxLength(50)]
        [Column("strRoleName", TypeName = "nvarchar(50)")]
        public string? StrRoleName { get; set; }

        [MaxLength(200)]
        [Column("strDescription", TypeName = "nvarchar(200)")]
        public string? StrDescription { get; set; }

        [Column("intLevel")]
        public int IntLevel { get; set; }

        [Column("intRoleUser")]
        public int? IntRoleUser { get; set; }
    }
}
