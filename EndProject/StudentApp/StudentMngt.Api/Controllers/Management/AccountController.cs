using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain;
using StudentMngt.Domain.Utility;
using StudentMngt.Domain.ApplicationServices.ClassesAS;

namespace StudentMngt.Api.Controllers.Management
{

    public class AccountController : AuthorizeController
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPut]
        [Route("update-info")]
        public async Task<ResponseResult> UpdateUserInfo([FromBody] UpdateUserInfoViewModel model)
        {
            var result = await _userService.UpdateUserInfo(model, CurrentUser);
            return result;
        }


        [HttpPut]
        [Route("change-password")]
        
        public async Task<ResponseResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            var result = await _userService.ChangePassword(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_USER_PERMISSION)]
        [HttpPost]
        [Route("register-system-user")]
        public async Task<ResponseResult> RegisterSystemUser([FromBody] RegisterUserViewModel model)
        {
            var result = await _userService.RegisterSystemUser(model);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_ROLE_PERMISSION)]
        [HttpPost]
        [Route("assign-role")]
        public async Task<ResponseResult> AssignRole([FromBody] AssignRolesViewModel model)
        {
            var result = await _userService.AssignRoles(model);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_ROLE_PERMISSION)]
        [HttpPost]
        [Route("remove-role")]
        public async Task<ResponseResult> RemoveRole([FromBody] RemoveRolesViewModel model)
        {
            var result = await _userService.RemoveRoles(model);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_ROLE_PERMISSION)]
        [HttpPost]
        [Route("assign-permissions")]
        public async Task<ResponseResult> AssignPermissions([FromBody] AssignPermissionsViewModel model)
        {
            var result = await _userService.AssignPermissions(model);
            return result;
        }

        [Permission(CommonConstants.Permissions.VIEW_USER_PERMISSION)]
        [HttpGet]
        [Route("get-users")]
        public async Task<PageResult<UserViewModel>> GetUsers([FromBody] UserSearchQuery model)
        {
            var result = await _userService.GetUsers(model);
            return result;
        }

        [Permission(CommonConstants.Permissions.VIEW_USER_PERMISSION)]
        [HttpPost]
        [Route("get-users-by-rolename")]
        public async Task<PageResult<UserViewModel>> GetUserByRoleName([FromBody] UserSearchQuery model)
        {
            var result = await _userService.GetUserByRoleName(model);
            return result;
        }

        //[Permission(CommonConstants.Permissions.VIEW_ROLE_PERMISSION)]
        [HttpGet]
        [Route("get-roles")]
        public async Task<PageResult<RoleViewModel>> GetRoles([FromBody] RoleSearchQuery model)
        {
            var result = await _userService.GetRoles(model);
            return result;
        }


        //[Permission(CommonConstants.Permissions.VIEW_ROLE_PERMISSION)]
        [HttpGet]
        [Route("get-roles-by-user")]
        public async Task<IList<String>> GetRolesByUser()
        {
            var result = await _userService.GetRolesByUser(UserName);
            return result;
        }




        [Permission(CommonConstants.Permissions.VIEW_ROLE_PERMISSION)]
        [HttpGet]
        [Route("get-role-detail")]
        public async Task<RoleViewModel> GetRoleDetail([FromQuery] String roleName)
        {
            var result = await _userService.GetRoleDetailByName(roleName);
            return result;
        }


        // Taoj role 
        [Permission(CommonConstants.Permissions.ADD_ROLE_PERMISSION)]
        [HttpPost]
        [Route("create-role")]
        public async Task<ResponseResult> CreateRole([FromBody]CreateRoleViewModel model)
        {
            var result = await _userService.CreateRole(model);
            return result;
        }


        // Tạo User cos gawns role cho user
        [Permission(CommonConstants.Permissions.ADD_USER_PERMISSION)]
        [HttpPost]
        [Route("create-user")]
        public async Task<ResponseResult> CreateUser([FromBody] RegisterUserViewModel model)
        {
            var result = await _userService.CreateUser(model);
            return result;
        }

        // Get user by ClassID
        [Permission(CommonConstants.Permissions.VIEW_USER_PERMISSION)]
        [HttpPost]
        [Route("get-users-by-classId")]
        public async Task<List<UserViewModel>> GetUsersByClassesId([FromBody] ClassesSearchQuery query)
        {
            var result = await _userService.GetListUserByClassId(query.Id);
            return result;
        }

    }
}
