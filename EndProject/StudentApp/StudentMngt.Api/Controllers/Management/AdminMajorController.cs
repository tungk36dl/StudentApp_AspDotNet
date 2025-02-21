using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.MajorAS;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class AdminMajorController : AuthorizeController
    {
        private readonly IMajorService _majorService;

        public AdminMajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        [HttpPost]
        [Route("get-all-major")]
        public async Task<PageResult<MajorViewModel>> GetAllMajor(MajorSearchQuery query)
        {
            query.UserId = CurrentUser.UserId;
            var result = await _majorService.GetMajors(query);
            return result;
        }

        [HttpPost]
        [Route("get-major")]
        public async Task<MajorViewModel> GetMajorById(MajorSearchQuery query)
        {
            var result = await _majorService.GetMajorById(query.Id);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_MAJOR_PERMISSION)]
        [HttpPost]
        [Route("create-major")]
        public async Task<ResponseResult> CreatMajor([FromBody] CreateMajorViewModel model)
        {
            var result = await _majorService.CreateMajor(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_MAJOR_PERMISSION)]
        [HttpPut]
        [Route("update-major")]
        public async Task<ResponseResult> UpdateMajor([FromBody] UpdateMajorViewModel model)
        {
            var result = await _majorService.UpdateMajor(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.DELETE_MAJOR_PERMISSION)]
        [HttpDelete]
        [Route("delete-major/{majorId}")]
        public async Task<ResponseResult> DeleteMajor(Guid majorId)
        {
            var result = await _majorService.DeleteMajor(majorId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_MAJOR_PERMISSION)]
        [HttpPut]
        [Route("update-major-status")]
        public async Task<ResponseResult> UpdateStatus([FromBody] UpdateStatusViewModel model)
        {
            var result = await _majorService.UpdateStatus(model);
            return result;
        }
    }
}
