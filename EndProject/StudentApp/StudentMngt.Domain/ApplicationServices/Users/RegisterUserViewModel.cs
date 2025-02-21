namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public String FullName { get; set; }

        public String DateOfBirth { get; set; }
        public Guid? ClassesId { get; set; }
        public string? Address { get; set; }
        public String? RoleName { get; set; }
    }
}
