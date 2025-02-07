using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.SubjectAS;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class AdminSubjectController : AuthorizeController
    {
        private readonly ISubjectService _subjectService;

        public AdminSubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        [Route("get-all-subject")]
        public async Task<PageResult<SubjectViewModel>> GetAllSubject(SubjectSearchQuery query)
        {
            query.UserId = CurrentUser.UserId;
            var result = await _subjectService.GetSubjects(query);
            return result;
        }

        [HttpGet]
        [Route("get-subject/{subjectId}")]
        public async Task<SubjectViewModel> GetSubjectById(Guid subjectId)
        {
            var result = await _subjectService.GetSubjectById(subjectId);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_SUBJECT_PERMISSION)]
        [HttpPost]
        [Route("create-subject")]
        public async Task<ResponseResult> CreatSubject([FromBody] CreateSubjectViewModel model)
        {
            var result = await _subjectService.CreateSubject(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_SUBJECT_PERMISSION)]
        [HttpPut]
        [Route("update-subject")]
        public async Task<ResponseResult> UpdateSubject([FromBody] UpdateSubjectViewModel model)
        {
            var result = await _subjectService.UpdateSubject(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.DELETE_SUBJECT_PERMISSION)]
        [HttpDelete]
        [Route("delete-subject/{subjectId}")]
        public async Task<ResponseResult> DeleteSubject(Guid subjectId)
        {
            var result = await _subjectService.DeleteSubject(subjectId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_SUBJECT_PERMISSION)]
        [HttpPut]
        [Route("update-subject-status")]
        public async Task<ResponseResult> UpdateStatus([FromBody] UpdateStatusViewModel model)
        {
            var result = await _subjectService.UpdateStatus(model);
            return result;
        }
    }
}
