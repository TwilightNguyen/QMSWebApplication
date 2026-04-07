using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Characteristic
{
    public class CharacteristicCreateRequest
    {
        public required string Name { get; set; }

        public required int MeaTypeId { get; set; }

        public required int ProcessId { get; set; }

        public required int DataType { get; set; }

        public string? Unit { get; set; }

        public int? DefectRateLimit { get; set; }

        public required int EmailEventModel { get; set; }

        public required int Decimals { get; set; }
    }
}
