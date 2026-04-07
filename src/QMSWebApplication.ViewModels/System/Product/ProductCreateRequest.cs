using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Product
{
    public class ProductCreateRequest
    {

        public required string Name { get; set; }

        public required int AreaId { get; set; }

        public required int InspPlanId { get; set; }

        public string? ModelInternal { get; set; }

        public string? CustomerName { get; set; }

        public string? Notes { get; set; }

        public string? Description { get; set; }

        public int? MoldQuanlity { get; set; }

        public int? CavityQuanlity { get; set; }
    }
}
