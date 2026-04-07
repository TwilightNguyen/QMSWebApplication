using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.MeasData
{
    public class MeasDataCreateRequest
    {
        public required int ProductionId { get; set; }

        public required int PlanTypeId { get; set; }

        public required int MoldId { get; set; }

        public required int CavityId { get; set; }

        public required List<MeasDataValue> Values { get; set; }

        public string? CharacteristicRange { get; set; }

        public string? Notes { get; set; }

        public int? SampleQunality { get; set; }
    }
}
