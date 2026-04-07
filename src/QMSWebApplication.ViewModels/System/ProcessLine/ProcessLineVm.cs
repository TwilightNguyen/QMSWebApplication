using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace QMSWebApplication.ViewModels.System.ProcessLine
{
    public class ProcesslineVm
    { 
        public required int Id { get; set; }
         
        public string? Name { get; set; } 

        public string? Code { get; set; }

        public int? ProcessId { get; set; }
    }
}
