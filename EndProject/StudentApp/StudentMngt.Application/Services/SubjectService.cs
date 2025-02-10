using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StudentMngt.Domain;
using StudentMngt.Domain.ApplicationServices.SubjectAS;
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

namespace StudentMngt.Application.Services
{
    public class SubjectService : ISubjectService
    {
        // private readonly ISubjectService _subjectService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Subject, Guid> _subjectRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SubjectService> _logger;

        public SubjectService(IUnitOfWork unitOfWork, IGenericRepository<Subject, Guid> subjectRepository, IHttpContextAccessor httpContextAccessor, ILogger<SubjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _subjectRepository = subjectRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ResponseResult> CreateSubject(CreateSubjectViewModel viewModel, UserProfileModel currentUser)
        {
            var subject = new Subject()
            {
                Id = Guid.NewGuid(),
                SubjectName = viewModel.SubjectName,
                Status = EntityStatus.Active,

                CreatedBy = currentUser.UserId,
                CreatedDate = DateTime.UtcNow,
            };
            try
            {
                _subjectRepository.Add(subject);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Create Subject {subject.SubjectName} successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectException.CreateSubjectException(viewModel.SubjectName);
            }
        }

        public async Task<ResponseResult> DeleteSubject(Guid subjectId)
        {
            var Subject = await _subjectRepository.FindByIdAsync(subjectId);
            if (Subject == null)
            {
                throw new SubjectException.SubjectNotFoundException(subjectId);
            }
            try
            {
                _subjectRepository.Remove(Subject);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Delete Subject with id {subjectId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectException.DeleteSubjectException(subjectId);
            }
        }

        public async Task<SubjectViewModel> GetSubjectById(Guid subjectId)
        {
            var subject = await _subjectRepository.FindByIdAsync(subjectId);
            if (subject == null)
            {
                throw new SubjectException.SubjectNotFoundException(subjectId);
            }
            var result = new SubjectViewModel()
            {
                Id = subject.Id,
                SubjectName = subject.SubjectName,
                Status = subject.Status,
            };
            return result;
        }

        public async Task<PageResult<SubjectViewModel>> GetSubjects(SubjectSearchQuery query)
        {
            var result = new PageResult<SubjectViewModel>()
            {
                CurrentPage = query.PageIndex
            };
            var subjectQuery = _subjectRepository.FindAll();
            // Chỉ hiển thị các subject đang active
            if (query.DisplayActiveItem)
            {
                subjectQuery = subjectQuery.Where(s => s.Status == EntityStatus.Active);
            }
            if (!String.IsNullOrEmpty(query.Keyword))
            {
                subjectQuery = subjectQuery.Where(s => s.SubjectName.Contains(query.Keyword));
            }
            result.TotalCount = await subjectQuery.CountAsync();
            result.Data = await subjectQuery
                .Skip(query.SkipNo).Take(query.TakeNo)
                .Select(s => new SubjectViewModel()
                {
                    Id = s.Id,
                    SubjectName = s.SubjectName,
                    Status = s.Status
                }).ToListAsync();

            return result;

        }

        public async Task<ResponseResult> UpdateStatus(UpdateStatusViewModel model)
        {
            var item = await _subjectRepository.FindByIdAsync(model.Id);
            if (item == null)
            {
                throw new SubjectException.SubjectNotFoundException(model.Id);
            }
            item.Status = model.Status;
            try
            {
                _subjectRepository.Update(item);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update status subject with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectException.UpdateSubjectException(model.Id);
            }
        }

        public async Task<ResponseResult> UpdateSubject(UpdateSubjectViewModel model, UserProfileModel currentUser)
        {
            var subject = await _subjectRepository.FindByIdAsync(model.Id);
            if (subject == null)
            {
                throw new SubjectException.UpdateSubjectException(model.Id);
            }

            subject.SubjectName = model.SubjectName;
            try
            {
                _subjectRepository.Update(subject);
                await _unitOfWork.SaveChangesAsync();
                return ResponseResult.Success($"Update Subject with id {model.Id} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw new SubjectException.UpdateSubjectException(model.Id);
            }
        }
    }
}
