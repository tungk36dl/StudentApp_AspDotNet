using StudentMngt.Domain.ApplicationServices.Users;

namespace StudentMngt.Domain.ApplicationServices.ClassesAS
{
    public interface IClassesService
    {
        Task<ResponseResult> CreateClasses(CreateClassesViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateClasses(UpdateClassesViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteClasses(Guid ClassesId);
        Task<PageResult<ClassesViewModel>> GetClassess(ClassesSearchQuery query);
        Task<List<ClassesViewModel>> GetAllClasses();
        Task<ClassesViewModel> GetClassesById(Guid ClassesId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);
    }
}
