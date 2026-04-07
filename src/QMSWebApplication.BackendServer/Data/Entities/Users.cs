//using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QMSWebApplication.BackendServer.Data.Entities
{  
    public class User : IdentityUser<string>
    {
        //public User()
        //{
        //    // Constructor logic (if any) goes here
        //}
        //public User(string id, string userName, string email, 
        //    int userId, string roleId, string password, string fullName, string department,
        //    string staffId, DateTime lastActivityTime, int enable, string selectedAreaId
        //)
        //{
        //    // Constructor logic (if any) goes here
        //    Id = id;
        //    UserName = userName;
        //    Email = email;
        //    NormalizedUserName = userName.ToUpperInvariant();
        //    NormalizedEmail = email.ToUpperInvariant();

        //    IntUserID = userId;
        //    StrFullName = fullName;
        //    StrRoleID = roleId;
        //    StrPassword = password;
        //    StrDepartment = department;
        //    StrStaffID = staffId;
        //    DtLastActivityTime = lastActivityTime;
        //    IntEnable = enable;
        //    StrSelectedAreaID = selectedAreaId;
        //    StrEmailAddress = email;
        //}

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
