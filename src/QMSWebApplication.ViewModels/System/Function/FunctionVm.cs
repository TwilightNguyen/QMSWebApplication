using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Function
{
    public class FunctionVm
    { 
        public required int Id { get; set; }

        public required string Name { get; set; }

        public required string Url { get; set; }

        public int? OrderNumber { get; set; }

        public int? ParentId { get; set; }

        public DateTimeOffset? UploadedDateTime { get; set; }

        public DateTimeOffset? ModifiedDateTime { get; set; }

        public bool Enabled { get; set; } = false;
    }
}
