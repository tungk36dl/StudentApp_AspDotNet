namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNummber { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
        public Guid? ClassesId { get; set; }
        public string? Address { get; set; }
    }
}
