using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;

namespace StudentMngt.Domain.ApplicationServices.ScoreAS
{
    public interface IScoreService
    {
        //Task<ResponseResult> CreateScore(CreateScoreViewModel viewModel, UserProfileModel currentUser);
        Task<ResponseResult> UpdateScore(List<UpdateScoreViewModel> scoreUpdates);
        //Task<ResponseResult> DeleteScore(Guid scoreId);
        //Task<PageResult<ScoreViewModel>> GetScores(ScoreSearchQuery query);
        //Task<ScoreViewModel> GetScoreById(Guid scoreId);
        //Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model);

        // Lấy danh sách điểm theo mã sinh viên
        Task<List<Score>> GetScoresByUserId(Guid userId);
        // Lấy danh sách điểm theo mã chi tiết môn học 
        Task<List<Score>> GetScoresBySubjectDetailId(Guid subjectDetailId);
    }
}
