using StudentMngt.Domain.ApplicationServices.SubjectAS;
using StudentMngt.Domain.ApplicationServices.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectDetailAS
{
    public interface ISubjectDetailService
    {
        Task<ResponseResult> CreateSubjectDetail(CreateSubjectDetailViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateSubjectDetail(UpdateSubjectDetailViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteSubjectDetail(Guid subjectId);
        Task<PageResult<SubjectDetailViewModel>> GetSubjectDetails(SubjectDetailSearchQuery query);
        Task<SubjectDetailViewModel> GetSubjectDetailById(Guid subjectId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);
    }
}
