using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.ScoreAS;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Enums;
using StudentMngt.Domain.Exceptions;

namespace StudentMngt.Application.Services
{
    public class ScoreService : IScoreService
    {
         private readonly IScoreService _scoreService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Score, Guid> _scoreRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ScoreService> _logger;

        public ScoreService(IScoreService scoreService, IUnitOfWork unitOfWork, IGenericRepository<Score, Guid> scoreRepository, IHttpContextAccessor httpContextAccessor, ILogger<ScoreService> logger)
        {
            _scoreService = scoreService;
            _unitOfWork = unitOfWork;
            _scoreRepository = scoreRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }





        public async Task<ResponseResult> CreateScore(List<CreateScoreViewModel> scores, UserProfileModel currentUser)
        {
            if (scores == null || !scores.Any())
            {
                throw new ScoreException.CreateListScoreException();
            }
               

            var newScores = scores.Select(viewModel => new Score()
            {
                Id = Guid.NewGuid(),
                UserId = viewModel.UserId,
                SubjectDetailId = viewModel.SubjectDetailId,
                AttendanceScore = viewModel.AttendanceScore,
                TestScore = viewModel.TestScore,
                Status = EntityStatus.Active,
                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow
            }).ToList();

            // Tính toán FinalScore, GPA, LetterGrade nếu có đủ dữ liệu
            foreach (var score in newScores)
            {
                CalculateFinalScore(score);
            }

            try
            {
                _scoreRepository.AddRange(newScores);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success("Đã lưu điểm thành công!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.CreateListScoreException();
            }
        }


        public async Task<ResponseResult> DeleteScore(Guid scoreId)
        {
            var Score = await _scoreRepository.FindByIdAsync(scoreId);
            if (Score == null)
            {
                throw new ScoreException.ScoreNotFoundException(scoreId);
            }
            try
            {
                _scoreRepository.Remove(Score);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete Score with id {scoreId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.DeleteScoreException(scoreId);
            }
        }

        public async Task<ScoreViewModel> GetScoreById(Guid scoreId)
        {
            var score = await _scoreRepository.FindByIdAsync(scoreId);
            if (score == null)
            {
                throw new ScoreException.ScoreNotFoundException(scoreId);
            }
            var result = new ScoreViewModel()
            {
                Id = score.Id,
                AttendanceScore = score.AttendanceScore,
                TestScore = score.TestScore,
                FinalScore = score.FinalScore,
                GPA = score.GPA,
                LetterGrades = score.LetterGrades,
                Status = score.Status,
            };
            return result;
        }

        public async Task<PageResult<ScoreViewModel>> GetScores(ScoreSearchQuery query)
        {
            var result = new PageResult<ScoreViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var scoreQuery = _scoreRepository.FindAll();
            // Chỉ hiển thị các score đang active
            if (query.DisplayActiveItem)
            {
                scoreQuery = scoreQuery.Where(s => s.Status == EntityStatus.Active);
            }
            //if (!String.IsNullOrEmpty(query.Keyword))
            //{
            //    scoreQuery = scoreQuery.Where(s => s.ScoreValue.Contains(query.Keyword));
            //}


            result.TotalCount = await scoreQuery.CountAsync();
            result.Data = await scoreQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new ScoreViewModel()
                {
                    Id = s.Id,
                    AttendanceScore = s.AttendanceScore,
                    TestScore = s.TestScore,
                    FinalScore = s.FinalScore,
                    GPA = s. GPA,
                    LetterGrades = s.LetterGrades,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _scoreRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new ScoreException.ScoreNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _scoreRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status score with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.UpdateScoreException(model.Id);
            }
        }

        //public async Task<ResponseResult> UpdateScore(UpdateScoreViewModel model, UserProfileModel currentUser)
        //{
        //    var score = await _scoreRepository.FindByIdAsync(model.Id);
        //    if (score == null)
        //    {
        //        throw new ScoreException.UpdateScoreException(model.Id);
        //    }

        //    score.AttendanceScore = model.AttendanceScore;
        //    try
        //    {
        //        _scoreRepository.Update(score);
        //        await _unitOfWork.SaveChangesAsync();
        //        return ResponseResult.Success($"Update Score with id {model.Id} successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //        throw new ScoreException.UpdateScoreException(model.Id);
        //    }
        //}

        public async Task<ResponseResult> UpdateScore(List<UpdateScoreViewModel> scoreUpdates)
        {
            if (scoreUpdates == null || !scoreUpdates.Any())
            {
                throw new ScoreException.HandleScoreException("List score update null!!!");
            }
                

            var subjectDetailId = scoreUpdates.First().SubjectDetailId;
            var existingScores = await _scoreService.GetScoresBySubjectDetailId(subjectDetailId);

            if (!existingScores.Any())
                throw new ScoreException.HandleScoreException("Not found data list score to update!!!");

            foreach (var existingScore in existingScores)
            {
                var updatedScore = scoreUpdates.FirstOrDefault(s => s.UserId == existingScore.UserId);
                if (updatedScore == null) continue;

                existingScore.AttendanceScore = updatedScore.AttendanceScore;
                existingScore.TestScore = updatedScore.TestScore;

                // Tính lại FinalScore, GPA, LetterGrade nếu đủ dữ liệu
                CalculateFinalScore(existingScore);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success("Cập nhật điểm thành công!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.HandleScoreException("Error when update list score ");
            }
        }


        public async Task<List<Score>> GetScoresByUserId(Guid userId)
        {
            var scores =  _scoreRepository.FindAll().Where(x => x.UserId == userId);
            if (scores.Any())
            {
                return new List<Score>();
            }
            return scores.ToList();
        }

        public async Task<List<Score>> GetScoresBySubjectDetailId(Guid subjectDetailId)
        {
            var scores = _scoreRepository.FindAll().Where(x => x.SubjectDetailId == subjectDetailId);
            if (scores.Any())
            {
                return new List<Score>();
            }
            return scores.ToList();
        }



        private void CalculateFinalScore(Score score)
        {
            if (score.AttendanceScore.HasValue && score.TestScore.HasValue)
            {
                // Tính điểm cuối kỳ dựa trên trọng số (ví dụ: 30% Attendance, 70% Exam)
                score.FinalScore = (score.AttendanceScore.Value * 0.3) + (score.TestScore.Value * 0.7);

                // Tính GPA theo thang điểm 4
                score.GPA = ConvertToGPA(score.FinalScore.Value);

                // Xếp loại điểm chữ (LetterGrade)
                score.LetterGrades = ConvertToLetterGrade(score.FinalScore.Value);
            }
        }

        private double ConvertToGPA(double finalScore)
        {
            if (finalScore >= 8.5) return 4.0;
            if (finalScore >= 7.0) return 3.5;
            if (finalScore >= 5.0) return 3.0;
            if (finalScore >= 3.5) return 2.0;
            return 1.0;
        }

        private LetterGrades ConvertToLetterGrade(double finalScore)
        {
            if (finalScore >= 8.5) return LetterGrades.A;
            if (finalScore >= 7.0) return LetterGrades.B;
            if (finalScore >= 5.0) return LetterGrades.C;
            if (finalScore >= 3.5) return LetterGrades.D;
            return LetterGrades.F;
        }

    }
}
