using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Exceptions;
using StudentMngt.Domain.InfrastructureServices;
using StudentMngt.Domain.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;

namespace StudentMngt.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IGenericRepository<Permission, Guid> _permissionRepository;
        private readonly IGenericRepository<RolePermission, Guid> _rolePermissionRepository;
        private readonly IGenericRepository<Classes, Guid> _classesRepository;

        private readonly IJwtTokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IGenericRepository<Permission, Guid> permissionRepository, IGenericRepository<RolePermission, Guid> rolePermissionRepository, IGenericRepository<Classes, Guid> classesRepository, IJwtTokenService tokenService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionRepository = permissionRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _classesRepository = classesRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }





        #region Common
        // Đăng nhập
        public async Task<AuthorizedResponseModel> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!checkPassword)
            {
                throw new UserException.PasswordNotCorrectException();
            }

            var claims = new List<Claim>
            {
                new("UserName", user.UserName),
                new(ClaimTypes.Email, user.Email)

            };
            var response = new AuthorizedResponseModel() { JwtToken = _tokenService.GenerateAccessToken(claims) };
            return response;

        }

        // Lấy thông tin user
        public async Task<UserProfileModel> GetUserProfile(string userName)
        {

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var result = new UserProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Code = user.Code,
                PhoneNumber = user.PhoneNumber
            };

            if (!user.IsSystemUser)
            {
                return result;
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || !userRoles.Any())
            {
                return result;
            }
            var roles = _roleManager.Roles;
            var permissions = _permissionRepository.FindAll();
            var rolePermissions = _rolePermissionRepository.FindAll();
            /* var userPermission =
                from r in roles
                join rp in rolePermissions on r.Id equals rp.RoleId
                join p in permissions on rp.PermissionCode equals p.Code
                select p.Code;
            */
            /* var userPermission =
               from r in roles
               join rp in rolePermissions on r.Id equals rp.RoleId
               select rp.PermissionCode;
            userPermission = userPermission.Where(s => userRoles.Contains(s));
            result.Permissions = userPermission.ToList().DistinctBy(s => s).ToList();
            */

            //select p.Code, r.Name from 
            //AspNetRoles as r
            //inner join
            //RolesPermissions as rp
            //on r.Id = rp.RoleId
            //inner join 
            //[dbo].[Permissions] as p
            //on p.Code = rp.PermissionCode select p.Code, r.Name from 
            //AspNetRoles as r
            //inner join
            //RolesPermissions as rp
            //on r.Id = rp.RoleId
            //inner join 
            //[dbo].[Permissions] as p
            //on p.Code = rp.PermissionCode
            var userPermission =
                from r in roles
                join rp in rolePermissions on r.Id equals rp.RoleId
                select new { rp.PermissionCode,   r.Name };

            var filerPermissions = userPermission.Where(s => userRoles.Contains(s.Name)).Select(x => x.PermissionCode).ToList();
            result.Permissions = filerPermissions.ToList().DistinctBy(s => s).ToList();

            return result;
        }


        // Cập nhật thông tin user
        public async Task<ResponseResult> UpdateUserInfo(UpdateUserInfoViewModel model, UserProfileModel currentUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == currentUser.UserId);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            user.Code = model.Code;
            user.Address = model.Address;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ResponseResult.Success("Update user profile success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }

        // Thay đổi password cho user
        public async Task<ResponseResult> ChangePassword(ChangePasswordViewModel model, UserProfileModel currentUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s=> s.Id == currentUser.UserId);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }


            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return ResponseResult.Success("Update user profile success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }


        #endregion

        #region Customers

        // Đăng kí 1 user
        public async Task<ResponseResult> RegisterCustomer(RegisterUserViewModel model)
        {
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNummber,
                Code = model.Code,
                Address = model.Address,
                ClassesId = model.ClassesId,
                IsSystemUser = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return ResponseResult.Success();
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }




        #endregion

        #region  System_Users
        // Đăng kí 1 user hệ thống
        public async Task<ResponseResult> RegisterSystemUser(RegisterUserViewModel model)
        {
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNummber,
                Code = model.Code,
                Address = model.Address,
                IsSystemUser = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return ResponseResult.Success();
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }

        // Đăng kí role 
        public async Task<ResponseResult> CreateRole(CreateRoleViewModel model)
        {
            var role = new AppRole()
            {
                Name = model.RoleName,
                NormalizedName = model.RoleName.ToUpper(),
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return ResponseResult.Success();
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }

        }


        // Add role cho user
        public async Task<ResponseResult> AssignRoles(AssignRolesViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }

            var result = await _userManager.AddToRolesAsync(user, model.RoleNames);
            if (result.Succeeded)
            {
                return ResponseResult.Success("Assign roles to user success");
            }
            var errors = JsonConvert.SerializeObject(result.Errors);
            throw new UserException.HandleUserException(errors);
        }


        // Xóa role cho user
        public async Task<ResponseResult> RemoveRoles(RemoveRolesViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }

            var result = await _userManager.RemoveFromRolesAsync(user, model.RoleNames);
            if (result.Succeeded)
            {
                return ResponseResult.Success();
            }
            var errors = JsonConvert.SerializeObject(result.Errors);
            throw new UserException.HandleUserException(errors);
        }


        // Lấy role từ userID 
        public async Task<IList<string>> GetRolesByUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user == null)
                {
                    throw new UserException.UserNotFoundException();
                }

                var roles = await _userManager.GetRolesAsync(user);
                return roles; // GetRolesAsync trả về IList<string> nên không cần convert
            }
            catch (UserException.UserNotFoundException)
            {
                throw; // Rethrow user not found exception
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                throw new Exception("Error getting user roles", ex);
            }
        }

        // Đăng kí lại quyền cho role
        public async Task<ResponseResult> AssignPermissions(AssignPermissionsViewModel model)

        {
            var permissions = model.Permissions.Where(s=> s.IsInRole).Select(s => new RolePermission()
            {
                Id = Guid.NewGuid(),
                RoleId = model.RoleId,
                PermissionCode = s.PermissionCode,
            }).ToList();
            var currentPermissions = await _rolePermissionRepository.FindAll(s => s.RoleId == model.RoleId).ToListAsync();
            try
            {
                //Băt đầu 1 phiên giao dịch , tức các hành động trong phiên xảy ra lần lượt liên tiếp nhau ko bị gián đoạn vởi hành động ngoài.
                // Khi 1 hành động bị lỗi sẽ chạy vào catch
                // Bản chất là chưa thực hiện vào DB, chỉ mới thực hiện vào vùng đệm 
                await _unitOfWork.BeginTransactionAsync();
                // Xóa các permission cũ 
                _rolePermissionRepository.RemoveMultiple(currentPermissions);
                // Add các permission mới
                _rolePermissionRepository.AddRange(permissions);
                // Mới lưu vào vùng đệm
                await _unitOfWork.SaveChangesAsync();
                // Câu lệnh này mới trưc tiếp thay đổi vào DB
                await _unitOfWork.CommitAsync();
                return ResponseResult.Success();
            }
            catch (Exception e)
            {
                // Hoàn lại như lúc ban đầu (giống như trước khi beginTransactionAsync() 
                await _unitOfWork.RollbackAsync();
                throw new UserException.HandleUserException("Somethings went wrong");
            }
        }

        // Lấy thông tin danh sách user
        public async Task<PageResult<UserViewModel>> GetUsers(UserSearchQuery query)
        {
            var result = new PageResult<UserViewModel>() { CurrentPage = query.PageIndex };
            var users = _userManager.Users.Where(s => s.IsSystemUser == query.IsSystemUser);
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                users = users.Where(s => s.UserName.Contains(query.Keyword)
                                    || s.Email.Contains(query.Keyword)
                                    || s.PhoneNumber.Contains(query.Keyword));
            }
            result.TotalCount = await users.CountAsync();
            result.Data = await users.Select(s => new UserViewModel
            {
                UserId = s.Id,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Code = s.Code,
                Address = s.Address ?? "",
                UserName = s.UserName,
            }).ToListAsync();
            return result;
        }

        // Lấy list user theo classes ID
        public async Task<List<UserViewModel>> GetListUserByClassId(Guid ClassesId)
        {
            if(ClassesId == Guid.Empty)
            {
                throw new UserException.HandleUserException("ClassID null!!!");
            }
            var classes = await _classesRepository.FindByIdAsync(ClassesId);
            if (classes == null)
            {
                throw new ClassesException.ClassesNotFoundException(ClassesId);
            }
            var students = _userManager.Users.Where(s => s.ClassesId == ClassesId);
            var result = await students.Select(s => new UserViewModel
            {
                UserId = s.Id,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                Code = s.Code,
                ClassName = classes.ClassName,
                Address = s.Address ?? "",
                UserName = s.UserName,
            }).ToListAsync();
            return  result;
        }


        // Lấy danh sách role
        public async Task<PageResult<RoleViewModel>> GetRoles(RoleSearchQuery query)
        {
            var result = new PageResult<RoleViewModel>() { CurrentPage = query.PageIndex };
            var roles = _roleManager.Roles;
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                roles = roles.Where(s => s.Name.Contains(query.Keyword));
            }
            result.TotalCount = await roles.CountAsync();

            result.Data = await roles.Select
                 (s => new RoleViewModel
                 {
                     RoleId = s.Id,
                     RoleName = s.Name
                 }).ToListAsync();
            return result;
        }


        public async Task<RoleViewModel> GetRoleDetail(Guid roleId)
        {
            var roles = _roleManager.Roles;
            var permissions = await _permissionRepository.FindAll().ToListAsync();
            var rolePermission = _rolePermissionRepository.FindAll();

            var role = roles.FirstOrDefault(s => s.Id == roleId);
            if (role == null)
            {
                throw new UserException.RoleNotFoundException();
            }

            /*
            var permissionCodesInRole = await (from r in roles
                    join rp in rolePermission on r.Id equals rp.RoleId
                    where r.Id == roleId
                    select rp.PermissionCode).ToListAsync();
            */
            var permissionCodesInRole = await rolePermission.Where(s => s.RoleId == roleId).Select(s => s.PermissionCode).ToListAsync();

            // Mapping cacs pẻmission thuộc role hiện tại với tất cả các permission
            var permissionViewModels = permissions.Select(s => new PermissionViewModel
            {
                // Nếu permission có tồn tại trong role hiện tịa thì íINRole = True
                IsInRole = permissionCodesInRole.Contains(s.Code),
                PermissionName = s.Name,
                PermissionCode = s.Code,
                ParentPermissionCode = s.ParentCode
            }).ToList();

            var usersInRole = (await _userManager.GetUsersInRoleAsync(role.Name)).Select(s=> new UserViewModel
            {
                UserId = s.Id,
                PhoneNumber = s.PhoneNumber,
                UserName = s.UserName,
                Code = s.Code,
                Address = s.Address ?? "",
                Email = s.Email
            }).ToList();
            var result = new RoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = permissionViewModels,
                UsersInRole = usersInRole
            };
            return result;
            
        }

        #endregion

        #region Seeding Data
        public async Task<bool> InitializeUserAdminAsync()
        {
            var userAdmin = _configuration.GetSection("UserAdmin");
            if (userAdmin != null)
            {
                var user = await _userManager.FindByNameAsync(userAdmin["UserName"]);
                if (user == null)
                {
                    var createUser = new AppUser()
                    {
                        UserName = userAdmin["UserName"],
                        Email = userAdmin["Email"],
                        Address = userAdmin["Address"],
                        Code = userAdmin["Code"],
                        IsSystemUser = true
                    };

                    var createUserResult = await _userManager.CreateAsync(createUser, userAdmin["Password"]);

                    if (!createUserResult.Succeeded)
                    {
                        return false;
                    }

                    var adminRole = new AppRole() { Name = userAdmin["Role"] };
                    var createRoleResult = await _roleManager.CreateAsync(adminRole);
                    if (!createRoleResult.Succeeded)
                    {
                        return false;
                    }

                    var assignRoleResult = await _userManager.AddToRoleAsync(createUser, adminRole.Name);
                    if (!assignRoleResult.Succeeded)
                    {
                        return false;
                    }

                    var listPermissions = new List<Permission>
                    {
                        // ROLE
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ROLE_PERMISSION,
                            Name = CommonConstants.Permissions.ROLE_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_ROLE_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.ROLE_PERMISSION,
                            Index = 2
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_ROLE_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.ROLE_PERMISSION,
                            Index = 3
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_ROLE_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.ROLE_PERMISSION,
                            Index = 4
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_ROLE_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.ROLE_PERMISSION,
                            Index = 5
                        }
                        ,

                        // USER
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.USER_PERMISSION,
                            Name = CommonConstants.Permissions.USER_PERMISSION,
                            Index = 1
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_USER_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_USER_PERMISSION,
                            ParentCode = CommonConstants.Permissions.USER_PERMISSION,
                            Index = 2
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_USER_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_USER_PERMISSION,
                            ParentCode = CommonConstants.Permissions.USER_PERMISSION,
                            Index = 3
                        }
                        ,

                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_USER_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_USER_PERMISSION,
                            ParentCode = CommonConstants.Permissions.USER_PERMISSION,
                            Index = 4
                        }
                        ,
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_USER_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_USER_PERMISSION,
                            ParentCode = CommonConstants.Permissions.USER_PERMISSION,
                            Index = 5
                        }
                        ,
                     

                        // IMAGE
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Name = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_IMAGE_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_IMAGE_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_IMAGE_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_IMAGE_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.IMAGE_PERMISSION,
                            Index = 5
                        },



                        // COHORT
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.COHORT_PERMISSION,
                            Name = CommonConstants.Permissions.COHORT_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_COHORT_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_COHORT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.COHORT_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_COHORT_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_COHORT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.COHORT_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_COHORT_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_COHORT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.COHORT_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_COHORT_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_COHORT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.COHORT_PERMISSION,
                            Index = 5
                        },


                        // MAJOR
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Name = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_MAJOR_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_MAJOR_PERMISSION,
                            ParentCode = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_MAJOR_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_MAJOR_PERMISSION,
                            ParentCode = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_MAJOR_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_MAJOR_PERMISSION,
                            ParentCode = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_MAJOR_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_MAJOR_PERMISSION,
                            ParentCode = CommonConstants.Permissions.MAJOR_PERMISSION,
                            Index = 5
                        },

                        // CLASSES
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Name = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_CLASSES_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_CLASSES_PERMISSION,
                            ParentCode = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_CLASSES_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_CLASSES_PERMISSION,
                            ParentCode = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_CLASSES_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_CLASSES_PERMISSION,
                            ParentCode = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_CLASSES_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_CLASSES_PERMISSION,
                            ParentCode = CommonConstants.Permissions.CLASSES_PERMISSION,
                            Index = 5
                        },


                        // SUBJECT
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Name = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_SUBJECT_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_SUBJECT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_SUBJECT_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_SUBJECT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_SUBJECT_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_SUBJECT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_SUBJECT_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_SUBJECT_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_PERMISSION,
                            Index = 5
                        },


                        // SUBJECT_DETAIL
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Name = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_SUBJECT_DETAIL_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_SUBJECT_DETAIL_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_SUBJECT_DETAIL_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_SUBJECT_DETAIL_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_SUBJECT_DETAIL_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_SUBJECT_DETAIL_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_SUBJECT_DETAIL_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_SUBJECT_DETAIL_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SUBJECT_DETAIL_PERMISSION,
                            Index = 5
                        },


                        // SCORE
                         new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.SCORE_PERMISSION,
                            Name = CommonConstants.Permissions.SCORE_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.VIEW_SCORE_PERMISSION,
                            Name = CommonConstants.Permissions.VIEW_SCORE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SCORE_PERMISSION,
                            Index = 2
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.ADD_SCORE_PERMISSION,
                            Name = CommonConstants.Permissions.ADD_SCORE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SCORE_PERMISSION,
                            Index = 3
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.UPDATE_SCORE_PERMISSION,
                            Name = CommonConstants.Permissions.UPDATE_SCORE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SCORE_PERMISSION,
                            Index = 4
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Permissions.DELETE_SCORE_PERMISSION,
                            Name = CommonConstants.Permissions.DELETE_SCORE_PERMISSION,
                            ParentCode = CommonConstants.Permissions.SCORE_PERMISSION,
                            Index = 5
                        }
                    };

                    var rolesPermissons = listPermissions.Select(s => new RolePermission()
                    {
                        Id = Guid.NewGuid(),
                        RoleId = adminRole.Id,
                        PermissionCode = s.Code,
                    }).ToList();

                    bool assignPermissionResult = true;

                    try
                    {
                        await _unitOfWork.BeginTransactionAsync();
                        _permissionRepository.AddRange(listPermissions);
                        _rolePermissionRepository.AddRange(rolesPermissons);
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await _unitOfWork.RollbackAsync();
                        assignPermissionResult = false;
                    }

                    if (!assignPermissionResult)
                    {
                        return false;
                    }
                    return true;
                }
            }

            else
            {
                return false;
            }

            return false;
        }


        #endregion
    }
}
