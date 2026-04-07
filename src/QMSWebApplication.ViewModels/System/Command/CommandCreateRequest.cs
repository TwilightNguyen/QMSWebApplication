using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Command
{
    public class CommandCreateRequest
    {
        public required string Name { get; set; }

        public string? Notes { get; set; }
    }
}
