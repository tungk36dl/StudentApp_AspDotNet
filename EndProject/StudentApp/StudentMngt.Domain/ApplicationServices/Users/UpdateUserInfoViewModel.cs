namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class UpdateUserInfoViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string? Address { get; set; }
    }
}
