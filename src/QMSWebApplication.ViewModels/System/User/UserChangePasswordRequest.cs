namespace QMSWebApplication.ViewModels.System.User
{
    public class UserChangePasswordRequest
    {
        public required int UserId { get; set; }
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
