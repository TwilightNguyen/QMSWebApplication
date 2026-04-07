using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.InspectionPlanSub
{
    public class InspectionPlanSubCreateRequest
    {
        public required int InspPlanId { get; set; }
        public required int PlanTypeId { get; set; }
    }
}
