using Microsoft.AspNetCore.Http;
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
        // private readonly IScoreService _scoreService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Score, Guid> _scoreRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ScoreService> _logger;

        public ScoreService(IUnitOfWork unitOfWork, IGenericRepository<Score, Guid> scoreRepository, IHttpContextAccessor httpContextAccessor, ILogger<ScoreService> logger)
        {
            _unitOfWork = unitOfWork;
            _scoreRepository = scoreRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateScore(CreateScoreViewModel viewModel, UserProfileModel currentUser)
        {
            var score = new Score()
            {
                Id = Guid.NewGuid(),
                ScoreValue = viewModel.ScoreValue,
                UserId = viewModel.UserId,
                SubjectDetailId = viewModel.SubjectDetailId,
                TypeScore = viewModel.TypeScore,
                Status = EntityStatus.Active,
                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _scoreRepository.Add(score);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create Score for userId: {score.UserId} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.CreateScoreException(viewModel.UserId);
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
                ScoreValue = score.ScoreValue,
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
                    ScoreValue = s.ScoreValue,
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

        public async Task<ResponseResult> UpdateScore(UpdateScoreViewModel model, UserProfileModel currentUser)
        {
            var score = await _scoreRepository.FindByIdAsync(model.Id);
            if (score == null)
            {
                throw new ScoreException.UpdateScoreException(model.Id);
            }

            score.ScoreValue = model.ScoreValue;
            try
            {
                _scoreRepository.Update(score);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update Score with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ScoreException.UpdateScoreException(model.Id);
            }
        }

        public async Task<List<ScoreViewModel>> GetScoresByUserId(Guid userId)
        {
            var scores =  _scoreRepository.FindAll().Where(x => x.UserId == userId);
            if (scores.Any())
            {
                return new List<ScoreViewModel>();
            }
            return scores.Select(score => new ScoreViewModel
            {
                ScoreValue = score.ScoreValue,
                Id = score.Id,
                UserId = userId,
                SubjectDetailId = score.SubjectDetailId,
            }).ToList();
        }

        public async Task<List<ScoreViewModel>> GetScoresBySubjectDetailId(Guid subjectDetailId)
        {
            var scores = _scoreRepository.FindAll().Where(x => x.SubjectDetailId == subjectDetailId);
            if (scores.Any())
            {
                return new List<ScoreViewModel>();
            }
            return scores.Select(score => new ScoreViewModel
            {
                Id = score.Id,
                UserId = score.UserId,
                SubjectDetailId = score.SubjectDetailId,
                ScoreValue = score.ScoreValue,
            }).ToList();
        }
    }
}
