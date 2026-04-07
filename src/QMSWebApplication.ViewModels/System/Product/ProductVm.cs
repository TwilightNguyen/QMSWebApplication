using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Product
{
    public class ProductVm
    {
        public required int Id { get; set; }

        public string? Name { get; set; }

        public int? AreaId { get; set; }

        public int? InspPlanId { get; set; }

        public string? ModelInternal { get; set; }

        public string? CustomerName { get; set; }

        public string? Notes { get; set; }

        public string? Description { get; set; }

        public int? MoldQuanlity { get; set; }

        public int? CavityQuanlity { get; set; }

    }
}
