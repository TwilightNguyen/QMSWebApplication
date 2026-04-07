using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Role
{
    public class RoleCreateRequest
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int RoleUser { get; set; }
        public int Level { get; set; }
    }
}
