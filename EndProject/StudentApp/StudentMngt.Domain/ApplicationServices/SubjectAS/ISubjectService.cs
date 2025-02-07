using StudentMngt.Domain.ApplicationServices.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentMngt.Domain.ApplicationServices.SubjectAS
{
    public interface ISubjectService
    {
        Task<ResponseResult> CreateSubject(CreateSubjectViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateSubject(UpdateSubjectViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteSubject(Guid subjectId);
        Task<PageResult<SubjectViewModel>> GetSubjects(SubjectSearchQuery query);
        Task<SubjectViewModel> GetSubjectById(Guid subjectId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);
    }
}
