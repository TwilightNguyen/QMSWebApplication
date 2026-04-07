using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Job
{
    public class JobCreateRequest
    {
        public required int AreaId { get; set; }

        public required int ProductId { get; set; }

        public required string JobCode { get; set; }

        public required string POCode { get; set; }

        public required string SOCode { get; set; }

        public required int OutputQuanlity { get; set; }

        public required int PlannedQuantity { get; set; }
    }
}
