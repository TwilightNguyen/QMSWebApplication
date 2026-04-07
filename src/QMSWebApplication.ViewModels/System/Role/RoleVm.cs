namespace QMSWebApplication.ViewModels.System.Role
{
    public class RoleVm
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int RoleUser { get; set; }
        public int Level { get; set; }
        public int RoleId { get; set; }
    }
}
