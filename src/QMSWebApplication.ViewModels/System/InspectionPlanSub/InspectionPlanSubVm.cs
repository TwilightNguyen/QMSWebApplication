using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.InspectionPlanSub
{
    public class InspectionPlanSubVm
    {
        public int Id { get; set; }
        public int? InspPlanId { get; set; }
        public int? PlanTypeId { get; set; }
        public DateTimeOffset? UploadedDateTime { get; set; }
    }
}
