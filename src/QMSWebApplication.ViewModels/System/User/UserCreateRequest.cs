using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.User
{
    public class UserCreateRequest
    {

        public string? RoleId { get; set; }

        public required string EmailAddress { get; set; }

        public required string FullName { get; set; }

        public required string Department { get; set; }

        public required string StaffId { get; set; }

        public int? Enable { get; set; }

        public string? SelectedAreaId { get; set; }

        public required string Password { get; set; }
    }
}
