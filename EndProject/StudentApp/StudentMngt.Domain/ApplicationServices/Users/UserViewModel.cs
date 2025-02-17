namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName  { get; set; }
        public string DateOfBirth   { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string ClassName { get; set; }
    }
}
