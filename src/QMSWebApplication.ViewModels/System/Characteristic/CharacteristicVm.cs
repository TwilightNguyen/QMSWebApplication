using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Characteristic
{
    public class CharacteristicVm
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? MeaTypeId { get; set; }

        public int? ProcessId { get; set; }

        public int? DataType { get; set; }

        public string? Unit { get; set; }

        public int? DefectRateLimit { get; set; }

        public int? EmailEventModel { get; set; }

        public int? Decimals { get; set; }

        public bool? Enabled { get; set; }
    }
}
