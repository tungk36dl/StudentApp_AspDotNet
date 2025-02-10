using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.ClassesAS;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Enums;
using StudentMngt.Domain.Exceptions;

namespace StudentMngt.Application.Services
{
    public class ClassesService : IClassesService
    {
        // private readonly IClassesService _ClassesService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Classes, Guid> _ClassesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ClassesService> _logger;

        public ClassesService(IUnitOfWork unitOfWork, IGenericRepository<Classes, Guid> classesRepository, IHttpContextAccessor httpContextAccessor, ILogger<ClassesService> logger)
        {
            _unitOfWork = unitOfWork;
            _ClassesRepository = classesRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateClasses(CreateClassesViewModel viewModel, UserProfileModel currentUser)
        {
            var classes = new Classes()
            {
                Id = Guid.NewGuid(),
                ClassName = viewModel.ClassesName,
                MajorId = viewModel.MajorId,
                Status = EntityStatus.Active,
                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _ClassesRepository.Add(classes);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create Classes {classes.ClassName} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ClassesException.CreateClassesException(viewModel.ClassesName);
            }
        }

        public async Task<ResponseResult> DeleteClasses(Guid classesId)
        {
            var classes = await _ClassesRepository.FindByIdAsync(classesId);
            if (classes == null)
            {
                throw new ClassesException.ClassesNotFoundException(classesId);
            }
            try
            {
                _ClassesRepository.Remove(classes);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete Classes with id {classesId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ClassesException.DeleteClassesException(classesId);
            }
        }

        public async Task<ClassesViewModel> GetClassesById(Guid classesId)
        {
            var classes = await _ClassesRepository.FindByIdAsync(classesId);
            if (classes == null)
            {
                throw new ClassesException.ClassesNotFoundException(classesId);
            }
            var result = new ClassesViewModel()
            {
                Id = classes.Id,
                ClassesName = classes.ClassName,
                MajorId = classes.MajorId,
                Status = classes.Status,
            };
            return result;
        }

        public async Task<PageResult<ClassesViewModel>> GetClassess(ClassesSearchQuery query)
        {
            var result = new PageResult<ClassesViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var classesQuery = _ClassesRepository.FindAll();
            // Chỉ hiển thị các Classes đang active
            if (query.DisplayActiveItem)
            {
                classesQuery = classesQuery.Where(s => s.Status == EntityStatus.Active);
            }
            if (!String.IsNullOrEmpty(query.Keyword))
            {
                classesQuery = classesQuery.Where(s => s.ClassName.Contains(query.Keyword));
            }
            result.TotalCount = await classesQuery.CountAsync();
            result.Data = await classesQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new ClassesViewModel()
                {
                    Id = s.Id,
                    ClassesName = s.ClassName,
                    MajorId = s.MajorId,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _ClassesRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new ClassesException.ClassesNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _ClassesRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status Classes with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new ClassesException.UpdateClassesException(model.Id);
            }
        }

        public async Task<ResponseResult> UpdateClasses(UpdateClassesViewModel model, UserProfileModel currentUser)
        {
            var classes = await _ClassesRepository.FindByIdAsync(model.Id);
            if (classes == null)
            {
                throw new ClassesException.UpdateClassesException(model.Id);
            }

            classes.ClassName = model.ClassesName;
            classes.MajorId = model.MajorId;
            try
            {
                _ClassesRepository.Update(classes);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update Classes with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);   
                throw new ClassesException.UpdateClassesException(model.Id);
            }
        }
    }
}
