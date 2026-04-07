using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Command
{
    public class CommandVm
    {
        public required int Id { get; set; }

        public required string Name { get; set; }

        public string? Notes { get; set; }

        public DateTimeOffset? UploadedDateTime { get; set; }

        public DateTimeOffset? ModifiedDateTime { get; set; }

        public int DisplayOrder { get; set; }

        public bool Enabled { get; set; }
    }
}
