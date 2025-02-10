using StudentMngt.Domain.ApplicationServices.Users;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public interface IScoreService
    {
        Task<ResponseResult> CreateScore(CreateScoreViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateScore(UpdateScoreViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> DeleteScore(Guid scoreId);
        Task<PageResult<ScoreViewModel>> GetScores(ScoreSearchQuery query);
        Task<ScoreViewModel> GetScoreById(Guid scoreId);
        Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);

        // Lấy danh sách điểm theo mã sinh viên
        Task<List<ScoreViewModel>> GetScoresByUserId(Guid userId);
        // Lấy danh sách điểm theo mã chi tiết môn học 
        Task<List<ScoreViewModel>> GetScoresBySubjectDetailId(Guid subjectDetailId);
    }
}
