using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.SubjectDetailAS;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class SubjectDetailController : AuthorizeController
    {
        private readonly ISubjectDetailService _subjectDetailService;

        public SubjectDetailController(ISubjectDetailService subjectDetailService)
        {
            _subjectDetailService = subjectDetailService;
        }


        [HttpGet]
        [Route("get-all-subject-detail")]
        public async Task<PageResult<SubjectDetailViewModel>> GetAllSubjectDetail([FromBody] SubjectDetailSearchQuery query)
        {
            query.UserId = CurrentUser.UserId;
            var result = await _subjectDetailService.GetSubjectDetails(query);
            return result;
        }

        [HttpGet]
        [Route("get-subject-detail/{subjectDetailId}")]
        public async Task<SubjectDetailViewModel> GetSubjectDetailById(Guid subjectDetailId)
        {
            var result = await _subjectDetailService.GetSubjectDetailById(subjectDetailId);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_SUBJECT_DETAIL_PERMISSION)]
        [HttpPost]
        [Route("create-subject-detail")]
        public async Task<ResponseResult> CreateSubjectDetail([FromBody] CreateSubjectDetailViewModel model)
        {
            var result = await _subjectDetailService.CreateSubjectDetail(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_SUBJECT_DETAIL_PERMISSION)]
        [HttpPost]
        [Route("update-subject-detail")]
        public async Task<ResponseResult> UpdateSubjectDetail([FromBody] UpdateSubjectDetailViewModel model)
        {
            var result = await _subjectDetailService.UpdateSubjectDetail(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.DELETE_SUBJECT_DETAIL_PERMISSION)]
        [HttpDelete]
        [Route("delete-subject-detail/{subjectDetailId}")]
        public async Task<ResponseResult> DeleteSubjectDetail(Guid subjectDetailId)
        {
            var result = await _subjectDetailService.DeleteSubjectDetail(subjectDetailId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_SUBJECT_DETAIL_PERMISSION)]
        [HttpPut]
        [Route("update-subject-detail-status")]
        public async Task<ResponseResult> UpdateStatus([FromBody] UpdateStatusViewModel model)
        {
            var result = await _subjectDetailService.UpdateStatus(model);
            return result;
        }
    }
}
