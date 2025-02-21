namespace StudentMngt.Domain.ApplicationServices.Users
{
    public interface IUserService
    {
        #region Commons

        Task<AuthorizedResponseModel> RefreshToken(RefreshTokenRequest request);
        Task Logout(string userId);
        Task<AuthorizedResponseModel> Login(LoginViewModel model);
        Task<UserProfileModel> GetUserProfile(string userName);
        Task<bool> InitializeUserAdminAsync();
        Task<ResponseResult> UpdateUserInfo(UpdateUserInfoViewModel model, UserProfileModel currentUser);
        Task<ResponseResult> ChangePassword(ChangePasswordViewModel model, UserProfileModel currentUser);

        Task<List<UserViewModel>> GetListUserByClassId(Guid ClassesId);

        #endregion

        #region Customers
        Task<ResponseResult> RegisterCustomer(RegisterUserViewModel model);
        Task<ResponseResult> CreateUser(RegisterUserViewModel model);
        Task<IList<String>> GetRolesByUser(String userName);
        #endregion

        #region SystemUsers
        Task<ResponseResult> RegisterSystemUser(RegisterUserViewModel model);
        Task<ResponseResult> AssignRoles(AssignRolesViewModel model);

        Task<ResponseResult> RemoveRoles(RemoveRolesViewModel model);

        Task<ResponseResult> AssignPermissions(AssignPermissionsViewModel model);

        Task<PageResult<UserViewModel>> GetUsers(UserSearchQuery query);
        Task<PageResult<UserViewModel>> GetUserByRoleName(UserSearchQuery query);

        Task<PageResult<RoleViewModel>> GetRoles(RoleSearchQuery query);

        Task<RoleViewModel> GetRoleDetail(Guid roleId);

        Task<RoleViewModel> GetRoleDetailByName(String roleName);

        Task<ResponseResult> CreateRole(CreateRoleViewModel model);

        #endregion
    }
}
