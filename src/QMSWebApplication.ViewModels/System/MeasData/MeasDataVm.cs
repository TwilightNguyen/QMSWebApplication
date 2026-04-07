using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.MeasData
{
    public class MeasDataVm
    {
        public int Id { get; set; }

        public int? CharacteristicId { get; set; }

        public string? CharacteristicValue { get; set; }

        public string? CharacteristicRange { get; set; }

        public DateTimeOffset? UploadedDateTime { get; set; }

        public int? DataCollection { get; set; }

        public DateTimeOffset? MeasuredDateTime { get; set; }

        public int? JobId { get; set; }

        public int? ProductionId { get; set; }

        public int? LineId { get; set; }

        public int? UserId { get; set; }

        public int? OutputQuanlity { get; set; }

        public int? SampleIndex { get; set; }

        public int? Status { get; set; }

        public int? EmailSent { get; set; }

        public int? PlanTypeId { get; set; }

        public string? Notes { get; set; }

        public int? SampleQuanlity { get; set; }

        public int? MoldId { get; set; }

        public int? CavityId { get; set; }
    }
}
