using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.User
{
    public class UserVm
    {
        public int UserID { get; set; }

        public string? RoleID { get; set; }

        public required string EmailAddress { get; set; }

        public required string FullName { get; set; }

        public required string Department { get; set; }

        public required string StaffID { get; set; }

        public DateTimeOffset? LastActivityTime { get; set; }

        public int? Enabled { get; set; }

        public string? SelectedAreaID { get; set; }
    }
}
