using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.InspectionPlanTracking
{
    public class InspectionPlanTrackingVm
    { 
        public int Id { get; set; }

        public int? InspPlanId { get; set; }

        public int? PlanTypeId { get; set; }

        public int? PlanState { get; set; }

        public DateTimeOffset? UploadedDateTime { get; set; }

        public int? UserId { get; set; }

        // Additional property not in the entity
        public string? UserName { get; set; }
        public string? InspPlanName { get; set; }
        public string? PlanTypeName { get; set; }
    }
}
