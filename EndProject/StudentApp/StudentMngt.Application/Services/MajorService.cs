using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.MajorAS;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Enums;
using StudentMngt.Domain.Exceptions;

namespace StudentMngt.Application.Services
{
    public class MajorService : IMajorService
    {
        // private readonly IMajorService _MajorService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Major, Guid> _MajorRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<MajorService> _logger;

        public MajorService(IUnitOfWork unitOfWork, IGenericRepository<Major, Guid> majorRepository, IHttpContextAccessor httpContextAccessor, ILogger<MajorService> logger)
        {
            _unitOfWork = unitOfWork;
            _MajorRepository = majorRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateMajor(CreateMajorViewModel viewModel, UserProfileModel currentUser)
        {
            var major = new Major()
            {
                Id = Guid.NewGuid(),
                MajorName = viewModel.MajorName,
                Status = EntityStatus.Active,

                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _MajorRepository.Add(major);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create Major {major.MajorName} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new MajorException.CreateMajorException(viewModel.MajorName);
            }
        }

        public async Task<ResponseResult> DeleteMajor(Guid majorId)
        {
            var Major = await _MajorRepository.FindByIdAsync(majorId);
            if (Major == null)
            {
                throw new MajorException.MajorNotFoundException(majorId);
            }
            try
            {
                _MajorRepository.Remove(Major);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete Major with id {majorId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new MajorException.DeleteMajorException(majorId);
            }
        }

        public async Task<MajorViewModel> GetMajorById(Guid majorId)
        {
            var major = await _MajorRepository.FindByIdAsync(majorId);
            if (major == null)
            {
                throw new MajorException.MajorNotFoundException(majorId);
            }
            var result = new MajorViewModel()
            {
                Id = major.Id,
                MajorName = major.MajorName,
                Status = major.Status,
            };
            return result;
        }

        public async Task<PageResult<MajorViewModel>> GetMajors(MajorSearchQuery query)
        {
            var result = new PageResult<MajorViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var majorQuery = _MajorRepository.FindAll();
            // Chỉ hiển thị các Major đang active
            if (query.DisplayActiveItem)
            {
                majorQuery = majorQuery.Where(s => s.Status == EntityStatus.Active);
            }
            if (!String.IsNullOrEmpty(query.Keyword))
            {
                majorQuery = majorQuery.Where(s => s.MajorName.Contains(query.Keyword));
            }
            result.TotalCount = await majorQuery.CountAsync();
            result.Data = await majorQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new MajorViewModel()
                {
                    Id = s.Id,
                    MajorName = s.MajorName,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _MajorRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new MajorException.MajorNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _MajorRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status Major with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new MajorException.UpdateMajorException(model.Id);
            }
        }

        public async Task<ResponseResult> UpdateMajor(UpdateMajorViewModel model, UserProfileModel currentUser)
        {
            var marjor = await _MajorRepository.FindByIdAsync(model.Id);
            if (marjor == null)
            {
                throw new MajorException.UpdateMajorException(model.Id);
            }

            marjor.MajorName = model.MajorName;

            try
            {
                _MajorRepository.Update(marjor);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update Major with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);   
                throw new MajorException.UpdateMajorException(model.Id);
            }
        }
    }
}
