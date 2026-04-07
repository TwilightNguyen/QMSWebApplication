using System;
using System.Collections.Generic;
using System.Text;

namespace QMSWebApplication.ViewModels.System.Function
{
    public class FunctionCreateRequest
    {
        public required string Name { get; set; }

        public required string Url { get; set; }

        public int? ParentId { get; set; }
    }
}
