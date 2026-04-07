using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.EventLog
{
    public class EventLogVm
    {
        public required int Id { get; set; }

        public DateTimeOffset? EventTime { get; set; }

        public string? EventCode { get; set; }

        public string? Description { get; set; }

        public string? Station { get; set; }
    }
}
