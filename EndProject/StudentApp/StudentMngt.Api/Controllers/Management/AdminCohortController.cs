using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Application.Services;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.CohortAS;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class AdminCohortController : AuthorizeController
    {
        private readonly ICohortService _cohortService;

        public AdminCohortController(ICohortService cohortService)
        {
            _cohortService = cohortService;
        }

        [HttpPost]
        [Route("get-all-cohort")]
        public async Task<PageResult<CohortViewModel>> GetAllSubjecCohort([FromBody]CohortSearchQuery query)
        {
            query.UserId = CurrentUser.UserId;
            var result = await _cohortService.GetCohorts(query);
            return result;
        }

        [HttpGet]
        [Route("get-cohort-detail/{cohortId}")]
        public async Task<CohortViewModel> GetCohortById(Guid cohortId)
        {
            var result = await _cohortService.GetCohortById(cohortId);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_COHORT_PERMISSION)]
        [HttpPost]
        [Route("create-cohort")]
        public async Task<ResponseResult> CreateCohort([FromBody] CreateCohortViewModel model)
        {
            var result = await _cohortService.CreateCohort(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_COHORT_PERMISSION)]
        [HttpPut]
        [Route("update-cohort")]
        public async Task<ResponseResult> UpdateCohort([FromBody] UpdateCohortViewModel model)
        {
            var result = await _cohortService.UpdateCohort(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.DELETE_COHORT_PERMISSION)]
        [HttpDelete]
        [Route("delete-cohort/{cohortId}")]
        public async Task<ResponseResult> DeleteCohort(Guid cohortId)
        {
            var result = await _cohortService.DeleteCohort(cohortId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_COHORT_PERMISSION)]
        [HttpPut]
        [Route("update-cohort-status")]
        public async Task<ResponseResult> UpdateCohortStatus([FromBody]UpdateStatusViewModel model)
        {
            var result = await _cohortService.UpdateStatus(model);
            return result;
        }
    }
}
