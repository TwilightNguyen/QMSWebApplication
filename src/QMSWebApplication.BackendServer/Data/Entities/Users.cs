//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{
    [Table("Users")]  
    
    public class User : IdentityUser<string>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("intUserID")]
        public int IntUserID { get; set; }

        [MaxLength(255)]
        [Column("strRoleID", TypeName = "nvarchar(255)")]
        public string? StrRoleID { get; set; }
         
        [MaxLength(50)]
        [Column("strPassword", TypeName = "nvarchar(50)")]
        public string? StrPassword { get; set; }

        [MaxLength(100)]
        [Column("strEmailAddress", TypeName = "nvarchar(100)")]
        public string? StrEmailAddress { get; set; }

        [MaxLength(100)]
        [Column("strFullName", TypeName = "nvarchar(100)")]
        public string? StrFullName { get; set; }

        [MaxLength(100)]
        [Column("strDepartment", TypeName = "nvarchar(100)")]
        public string? StrDepartment { get; set; }

        [MaxLength(100)]
        [Column("strStaffID", TypeName = "nvarchar(100)")]
        public string? StrStaffID { get; set; }

        [Column("dtLastActivityTime", TypeName = "datetime")]
        public DateTime? DtLastActivityTime { get; set; }

        [Column("intEnable")]
        public int? IntEnable { get; set; }

        [MaxLength(50)]
        [Column("strSelectedAreaID", TypeName = "nvarchar(50)")]
        public string? StrSelectedAreaID { get; set; }
    }
}
