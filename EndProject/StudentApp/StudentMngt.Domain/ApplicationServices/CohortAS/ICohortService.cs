using StudentMngt.Domain.ApplicationServices.Users;

namespace StudentMngt.Domain.ApplicationServices.CohortAS
{
    public interface ICohortService
    {
        Task<ResponseResult> CreateCohort(CreateCohortViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateCohort(UpdateCohortViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteCohort(Guid CohortId);
        Task<PageResult<CohortViewModel>> GetCohorts(CohortSearchQuery query);
        Task<CohortViewModel> GetCohortById(Guid CohortId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);
    }
}
