using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.ScoreAS;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class AdminScoreController : AuthorizeController
    {
        private readonly IScoreService _scoreService;

        public AdminScoreController(IScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        [HttpGet]
        [Route("get-all-score")]
        public async Task<List<Score>> GetAllScore(ScoreSearchQuery query)
        {
            query.UserId = CurrentUser.UserId;
            var result = await _scoreService.GetScoresByUserId(query.UserId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_SCORE_PERMISSION)]
        [HttpPost]
        [Route("update-score-by-teacher")]
        public async Task<ResponseResult> UpdateScoresByTeacher(List<UpdateScoreViewModel> scoresUpdate)
        {
            var result = await _scoreService.UpdateScore(scoresUpdate);
            return result;
        }

        //[HttpGet]
        //[Route("get-all-score")]
        //public async Task<PageResult<ScoreViewModel>> GetAllScore(ScoreSearchQuery query)
        //{
        //    query.UserId = CurrentUser.UserId;
        //    var result = await _scoreService.GetScores(query);
        //    return result;
        //}

        //[HttpGet]
        //[Route("get-score/{scoreId}")]
        //public async Task<ScoreViewModel> GetScoreById(Guid scoreId)
        //{
        //    var result = await _scoreService.GetScoreById(scoreId);
        //    return result;
        //}

        //[HttpGet]
        //[Route("get-score-by-user/{userId}")]
        //public async Task<List<ScoreViewModel>> GetScoreByUserId(Guid userId)
        //{
        //    var result = await _scoreService.GetScoresByUserId(userId);
        //    return result;
        //}

        //[HttpGet]
        //[Route("get-score-by-subject-detail/{subjectDetailId}")]
        //public async Task<List<ScoreViewModel>> GetScoreBySubjectDetailId(Guid subjectDetailId)
        //{
        //    var result = await _scoreService.GetScoresBySubjectDetailId(subjectDetailId);
        //    return result;
        //}

        //[Permission(CommonConstants.Permissions.ADD_SCORE_PERMISSION)]
        //[HttpPost]
        //[Route("create-score")]
        //public async Task<ResponseResult> CreatScore([FromBody] CreateScoreViewModel model)
        //{
        //    var result = await _scoreService.CreateScore(model, CurrentUser);
        //    return result;
        //}

        //[Permission(CommonConstants.Permissions.UPDATE_SCORE_PERMISSION)]
        //[HttpPut]
        //[Route("update-score")]
        //public async Task<ResponseResult> UpdateScore([FromBody] UpdateScoreViewModel model)
        //{
        //    var result = await _scoreService.UpdateScore(model, CurrentUser);
        //    return result;
        //}

        //[Permission(CommonConstants.Permissions.DELETE_SCORE_PERMISSION)]
        //[HttpDelete]
        //[Route("delete-score/{scoreId}")]
        //public async Task<ResponseResult> DeleteScore(Guid scoreId)
        //{
        //    var result = await _scoreService.DeleteScore(scoreId);
        //    return result;
        //}

        //[Permission(CommonConstants.Permissions.UPDATE_SCORE_PERMISSION)]
        //[HttpPut]
        //[Route("update-score-status")]
        //public async Task<ResponseResult> UpdateStatus([FromBody] UpdateStatusViewModel model)
        //{
        //    var result = await _scoreService.UpdateStatus(model);
        //    return result;
        //}
    }
}
