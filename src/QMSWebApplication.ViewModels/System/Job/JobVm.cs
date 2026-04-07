namespace QMSWebApplication.ViewModels.System.Job
{
    public class JobVm
    {
        public int Id { get; set; }

        public int? AreaId { get; set; }

        public int? ProductId { get; set; }

        public string? JobCode { get; set; }

        public string? POCode { get; set; }

        public string? SOCode { get; set; }

        public int? PlannedQuanlity { get; set; }

        public int? OutputQuanlity { get; set; }

        public DateTimeOffset? UploadedDateTime { get; set; }

        public int? JobDecisionId { get; set; }

        public int? UserId { get; set; }
    }
}
