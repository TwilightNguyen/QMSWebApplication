using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.EventLog
{
    public class EventLogCreateRequest
    {
        public required string Event { get; set; }
    }
}
