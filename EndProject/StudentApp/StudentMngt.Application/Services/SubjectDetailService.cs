using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.Users;
using StudentMngt.Domain.Entities;
using StudentMngt.Domain.Exceptions;
using StudentMngt.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using StudentMngt.Domain.ApplicationServices.SubjectDetailAS;

namespace StudentMngt.Application.Services
{
    public class SubjectDetailService : ISubjectDetailService
    {
        // private readonly IsubjectDetailService _subjectDetailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<SubjectDetail, Guid> _subjectDetailRepository;
        private readonly IGenericRepository<Subject, Guid> _subjectRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SubjectDetail> _logger;

        public SubjectDetailService(IUnitOfWork unitOfWork, IGenericRepository<SubjectDetail, Guid> subjectDetailRepository, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<SubjectDetail> logger)
        {
            _unitOfWork = unitOfWork;
            _subjectDetailRepository = subjectDetailRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateSubjectDetail(CreateSubjectDetailViewModel viewModel, UserProfileModel currentUser)
        {
            if(viewModel.SubjectId == null || viewModel.TeacherId == null)
            {

            }
            var Teacher = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == viewModel.TeacherId);
            var Subject = await _subjectRepository.FindByIdAsync(viewModel.SubjectId);
            if (Teacher == null || Subject == null)
            {
                throw new SubjectDetailException.SubjectDetailNotFoundException(viewModel.SubjectId);
            }
            var subjectDetail = new SubjectDetail()
            {
                Id = Guid.NewGuid(),
                Credits = viewModel.Credits,
                SubjectId = viewModel.SubjectId,
                TeacherId = viewModel.TeacherId,
                Teacher = Teacher,
                Subject = Subject,

                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _subjectDetailRepository.Add(subjectDetail);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create subjectDetail {subjectDetail.SubjectId} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectDetailException.CreatesubjectDetailException(viewModel.SubjectId.ToString());
            }
        }

        public async Task<ResponseResult> DeleteSubjectDetail(Guid subjectDetailId)
        {
            var subjectDetail = await _subjectDetailRepository.FindByIdAsync(subjectDetailId);
            if (subjectDetail == null)
            {
                throw new SubjectDetailException.SubjectDetailNotFoundException(subjectDetailId);
            }
            try
            {
                _subjectDetailRepository.Remove(subjectDetail);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete subjectDetail with id {subjectDetailId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex );
                throw new SubjectDetailException.DeletesubjectDetailException(subjectDetailId);
            }
        }

        public async Task<SubjectDetailViewModel> GetSubjectDetailById(Guid subjectDetailId)
        {
            var subjectDetail = await _subjectDetailRepository.FindByIdAsync(subjectDetailId);
            if(subjectDetail == null)
            {
                throw new SubjectDetailException.SubjectDetailNotFoundException(subjectDetailId);
            }
           
            var result = new SubjectDetailViewModel()
            {
                SubjectDetailId = subjectDetailId,
                Credits = subjectDetail.Credits,

                TeacherName = subjectDetail.Teacher.FullName,
                SubjectName = subjectDetail.Subject.SubjectName
                
            };
            return result;
        }

        public async Task<PageResult<SubjectDetailViewModel>> GetSubjectDetails(SubjectDetailSearchQuery query)
        {
            var result = new PageResult<SubjectDetailViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var subjectDetailQuery = _subjectDetailRepository.FindAll();
            // Chỉ hiển thị các subjectDetail đang active
            if(query.DisplayActiveItem)
            {
                subjectDetailQuery = subjectDetailQuery.Where(s => s.Status == EntityStatus.Active);
            }
            if (!String.IsNullOrEmpty(query.Keyword))
            {
                //subjectDetailQuery = subjectDetailQuery.Where(s => s.subjectDetailName.Contains(query.Keyword));
            }
            result.TotalCount = await subjectDetailQuery.CountAsync();
            result.Data = await subjectDetailQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new SubjectDetailViewModel()
                {
                    SubjectDetailId = s.Id,
                    Credits = s.Credits,
                    TeacherName = s.Teacher.FullName,
                    SubjectName = s.Subject.SubjectName,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _subjectDetailRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new SubjectDetailException.SubjectDetailNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _subjectDetailRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status subjectDetail with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectDetailException.UpdatesubjectDetailException(model.Id);
            }
        }

        public async Task<ResponseResult> UpdateSubjectDetail(UpdateSubjectDetailViewModel model, UserProfileModel currentUser)
        {
            var subjectDetail = await _subjectDetailRepository.FindByIdAsync(model.SubjectDetailId);
            if (subjectDetail == null)
            {
                throw new SubjectDetailException.UpdatesubjectDetailException(model.SubjectDetailId);
            }

            subjectDetail.Credits = model.Credits;
            try
            {
                _subjectDetailRepository.Update(subjectDetail);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update subjectDetail with id {model.SubjectDetailId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectDetailException.UpdatesubjectDetailException(model.SubjectDetailId);
            }
        }
    }
}
