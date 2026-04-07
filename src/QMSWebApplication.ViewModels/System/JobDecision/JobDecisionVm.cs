using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.JobDecision
{
    public class JobDecisionVm
    {
        public int Id { get; set; }

        public string? Decision { get; set; }

        public string? ColorCode { get; set; }
    }
}
