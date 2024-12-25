using StudentMngt.Domain;

namespace StudentMngt.Domain.ApplicationServices.Users
{
    public class UserSearchQuery : SearchQuery
    {
        public bool IsSystemUser { get; set; }
    }
}
