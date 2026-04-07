using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.ProductionPlan
{
    public class ProductionPlanVm
    {
        public int Id { get; set; }

        public int? LineId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        public int? JobId { get; set; }

        public int? UserId { get; set; }

        public string? Notes { get; set; }
        
        public DateTimeOffset? ProductionDate { get; set; }

        public int? PlannedQuanlity { get; set; }

        public string? LotInform { get; set; }

        public string? MaterialInform { get; set; }

    }
}
