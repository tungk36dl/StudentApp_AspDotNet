using Microsoft.AspNetCore.Mvc;
using StudentMngt.Api.Base;
using StudentMngt.Api.Filters;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.ClassesAS;
using StudentMngt.Domain.Utility;

namespace StudentMngt.Api.Controllers.Management
{
    public class AdminClassesController : AuthorizeController
    {
        private readonly IClassesService _classesService;

        public AdminClassesController(IClassesService classesService)
        {
            _classesService = classesService;
        }


        [HttpPost]
        [Route("get-all-classes")]
        public async Task<PageResult<ClassesViewModel>> GetAllClasses(ClassesSearchQuery query)
        {
            query.Id = CurrentUser.UserId;
            var result = await _classesService.GetClassess(query);
            return result;
        }


        [Permission(CommonConstants.Permissions.VIEW_CLASSES_PERMISSION)]
        [HttpPost]
        [Route("get-classes")]
        public async Task<ClassesViewModel> GetClassesById(ClassesSearchQuery query)
        {
            var result = await _classesService.GetClassesById(query.Id);
            return result;
        }

        [Permission(CommonConstants.Permissions.ADD_CLASSES_PERMISSION)]
        [HttpPost]
        [Route("create-classes")]
        public async Task<ResponseResult> CreateClasses([FromBody] CreateClassesViewModel model)
        {
            var result = await _classesService.CreateClasses(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_CLASSES_PERMISSION)]
        [HttpPut]
        [Route("update-classes")]
        public async Task<ResponseResult> UpdateClasses([FromBody] UpdateClassesViewModel model)
        {
            var result = await _classesService.UpdateClasses(model, CurrentUser);
            return result;
        }

        [Permission(CommonConstants.Permissions.DELETE_CLASSES_PERMISSION)]
        [HttpDelete]
        [Route("delete-classes/{classesId}")]
        public async Task<ResponseResult> DeleteClasses(Guid classesId)
        {
            var result = await _classesService.DeleteClasses(classesId);
            return result;
        }

        [Permission(CommonConstants.Permissions.UPDATE_CLASSES_PERMISSION)]
        [HttpPut]
        [Route("update-classes-status")]
        public async Task<ResponseResult> UpdateStatus([FromBody] UpdateStatusViewModel model)
        {
            var result = await _classesService.UpdateStatus(model);
            return result;
        }
    }
}
