using StudentMngt.Domain.ApplicationServices.SubjectAS;
using StudentMngt.Domain.ApplicationServices.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.MajorAS
{
    public interface IMajorService
    {
        Task<ResponseResult> CreateMajor(CreateMajorViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateMajor(UpdateMajorViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteMajor(Guid MajorId);
        Task<PageResult<MajorViewModel>> GetMajors(MajorSearchQuery query);
        Task<MajorViewModel> GetMajorById(Guid MajorId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);
    }
}
