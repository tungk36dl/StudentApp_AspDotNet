using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.CohortAS;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Enums;
using StudentMngt.Domain.Exceptions;

namespace StudentMngt.Application.Services
{
    public class CohortService : ICohortService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Cohort, Guid> _CohortRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CohortService> _logger;

        public CohortService(IUnitOfWork unitOfWork, IGenericRepository<Cohort, Guid> cohortRepository, IHttpContextAccessor httpContextAccessor, ILogger<CohortService> logger)
        {
            _unitOfWork = unitOfWork;
            _CohortRepository = cohortRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateCohort(CreateCohortViewModel viewModel, UserProfileModel currentUser)
        {
            var cohort = new Cohort()
            {
                Id = Guid.NewGuid(),
                CohortName = viewModel.CohortName,
                CreatedBy = currentUser.UserId,
                Status = EntityStatus.Active,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _CohortRepository.Add(cohort);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create Cohort {cohort.CohortName} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new CohortException.CreateCohortException(viewModel.CohortName);
            }
        }

        public async Task<ResponseResult> DeleteCohort(Guid cohortId)
        {
            var cohort = await _CohortRepository.FindByIdAsync(cohortId);
            if (cohort == null)
            {
                throw new CohortException.CohortNotFoundException(cohortId);
            }
            try
            {
                _CohortRepository.Remove(cohort);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete Cohort with id {cohortId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new CohortException.DeleteCohortException(cohortId);
            }
        }

        public async Task<CohortViewModel> GetCohortById(Guid cohortId)
        {
            var cohort = await _CohortRepository.FindByIdAsync(cohortId);
            if (cohort == null)
            {
                throw new CohortException.CohortNotFoundException(cohortId);
            }
            var result = new CohortViewModel()
            {
                Id = cohort.Id,
                CohortName = cohort.CohortName,
                Status = cohort.Status,
            };
            return result;
        }

        public async Task<PageResult<CohortViewModel>> GetCohorts(CohortSearchQuery query)
        {
            var result = new PageResult<CohortViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var cohortQuery = _CohortRepository.FindAll();
            // Chỉ hiển thị các Cohort đang active
            if (query.DisplayActiveItem)
            {
                cohortQuery = cohortQuery.Where(s => s.Status == EntityStatus.Active);
            }
            if (!String.IsNullOrEmpty(query.Keyword))
            {
                cohortQuery = cohortQuery.Where(s => s.CohortName.Contains(query.Keyword));
            }

            cohortQuery = cohortQuery.OrderByDescending(s => s.CreatedDate);
            result.TotalCount = await cohortQuery.CountAsync();
            result.Data = await cohortQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new CohortViewModel()
                {
                    Id = s.Id,
                    CohortName = s.CohortName,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _CohortRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new CohortException.CohortNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _CohortRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status Cohort with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new CohortException.UpdateCohortException(model.Id);
            }
        }

        public async Task<ResponseResult> UpdateCohort(UpdateCohortViewModel model, UserProfileModel currentUser)
        {
            var cohort = await _CohortRepository.FindByIdAsync(model.Id);
            if (cohort == null)
            {
                throw new CohortException.UpdateCohortException(model.Id);
            }

            cohort.CohortName = model.CohortName;
            try
            {
                _CohortRepository.Update(cohort);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update Cohort with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);   
                throw new CohortException.UpdateCohortException(model.Id);
            }
        }
    }
}
