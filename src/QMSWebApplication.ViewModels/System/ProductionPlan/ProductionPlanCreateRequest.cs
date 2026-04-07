using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.ProductionPlan
{
    public class ProductionPlanCreateRequest
    {
        public required int LineId { get; set; }

        public required int JobId { get; set; }

        public string? Notes { get; set; }

        public DateTimeOffset? ProductionDate { get; set; }

        public int? PlannedQuantity { get; set; }

        public string? LotInform { get; set; }

        public string? MaterialInform { get; set; }
    }
}
