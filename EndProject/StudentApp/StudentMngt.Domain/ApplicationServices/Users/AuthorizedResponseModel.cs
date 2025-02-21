namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class AuthorizedResponseModel
    {
        public string  JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
