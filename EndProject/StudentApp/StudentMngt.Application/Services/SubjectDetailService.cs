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
        private readonly IGenericRepository<Classes, Guid> _classesRepository;
        private readonly IGenericRepository<Score, Guid> _scoreRepository;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SubjectDetailService> _logger;

        public SubjectDetailService(IUnitOfWork unitOfWork, IGenericRepository<SubjectDetail, Guid> subjectDetailRepository, IGenericRepository<Subject, Guid> subjectRepository, IGenericRepository<Classes, Guid> classesRepository, IGenericRepository<Score, Guid> scoreRepository, IUserService userService, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<SubjectDetailService> logger)
        {
            _unitOfWork = unitOfWork;
            _subjectDetailRepository = subjectDetailRepository;
            _subjectRepository = subjectRepository;
            _classesRepository = classesRepository;
            _scoreRepository = scoreRepository;
            _userService = userService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }





        //public async Task<ResponseResult> CreateSubjectDetail(CreateSubjectDetailViewModel viewModel, UserProfileModel currentUser)
        //{
        //    var Teacher = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == viewModel.TeacherId);
        //    var Subject = await _subjectRepository.FindByIdAsync(viewModel.SubjectId);
        //    if (Teacher == null || Subject == null)
        //    {
        //        throw new SubjectDetailException.SubjectDetailNotFoundException(viewModel.SubjectId);
        //    }

        //    var subjectDetail = new SubjectDetail()
        //    {
        //        Id = Guid.NewGuid(),
        //        Credits = viewModel.Credits,
        //        SubjectId = viewModel.SubjectId,
        //        TeacherId = viewModel.TeacherId,
        //        Teacher = Teacher,
        //        Subject = Subject,
        //        Status = EntityStatus.Active,
        //        ClassId = viewModel.ClassesId,
        //        CreatedBy = currentUser.UserId,
        //        CreatedDate = DateTime.UtcNow,
        //    };

        //    try
        //    {
        //        // Thêm subject detail vào database
        //        _subjectDetailRepository.Add(subjectDetail);
        //        _logger.LogDebug("Add SubjectDetail ....");
        //        await _unitOfWork.SaveChangesAsync();

        //        // Lấy danh sách sinh viên theo classId
        //        var students = await _userService.GetListUserByClassId(viewModel.ClassesId);

        //        if (students.Any())
        //        {
        //            var scores = students.Select(student => new Score
        //            {
        //                Id = Guid.NewGuid(),
        //                UserId = student.UserId,
        //                SubjectDetailId = subjectDetail.Id,
        //                AttendanceScore = null,
        //                TestScore = null,
        //                FinalScore = null,
        //                GPA = null,
        //                LetterGrades = null,
        //                Status = EntityStatus.Active,
        //                CreatedBy = currentUser.UserId,
        //                CreatedDate = DateTime.UtcNow
        //            }).ToList();

        //            _scoreRepository.AddRange(scores);
        //            await _unitOfWork.SaveChangesAsync();
        //        }else
        //        {
        //            throw new Exception($"Lớp có id: {viewModel.ClassesId} không có sinh viên");
        //        }

        //        return ResponseResult.Success($"Tạo môn học {subjectDetail.SubjectId} thành công và tự động thêm điểm rỗng cho sinh viên!");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex.Message, ex);
        //        throw new SubjectDetailException.CreatesubjectDetailException(viewModel.SubjectId.ToString());
        //    }
        //}


        public async Task<ResponseResult> CreateSubjectDetail(CreateSubjectDetailViewModel viewModel, UserProfileModel currentUser)
        {
            var Teacher = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == viewModel.TeacherId);
            var Subject = await _subjectRepository.FindByIdAsync(viewModel.SubjectId);
            if (Teacher == null || Subject == null)
            {
                throw new SubjectDetailException.SubjectDetailNotFoundException(viewModel.SubjectId);
            }

            // Lấy danh sách sinh viên theo classId trước khi tạo SubjectDetail
            var students = await _userService.GetListUserByClassId(viewModel.ClassesId);
            if (!students.Any())
            {
                throw new Exception($"Lớp có id: {viewModel.ClassesId} không có sinh viên");
            }

            // Bắt đầu transaction để đảm bảo toàn bộ quá trình thành công hoặc rollback nếu lỗi
   
                try
                {
                    await _unitOfWork.BeginTransactionAsync();
                    var subjectDetail = new SubjectDetail()
                    {
                        Id = Guid.NewGuid(),
                        Credits = viewModel.Credits,
                        SubjectId = viewModel.SubjectId,
                        TeacherId = viewModel.TeacherId,
                        Teacher = Teacher,
                        Subject = Subject,
                        Status = EntityStatus.Active,
                        ClassId = viewModel.ClassesId,
                        CreatedBy = currentUser.UserId,
                        CreatedDate = DateTime.UtcNow,
                    };

                    _subjectDetailRepository.Add(subjectDetail);
                    _logger.LogDebug("Add SubjectDetail ....");
                    await _unitOfWork.SaveChangesAsync();

                    var scores = students.Select(student => new Score
                    {
                        Id = Guid.NewGuid(),
                        UserId = student.UserId,
                        SubjectDetailId = subjectDetail.Id,
                        AttendanceScore = null,
                        TestScore = null,
                        FinalScore = null,
                        GPA = null,
                        LetterGrades = null,
                        Status = EntityStatus.Active,
                        CreatedBy = currentUser.UserId,
                        CreatedDate = DateTime.UtcNow
                    }).ToList();

                    _scoreRepository.AddRange(scores);
                    await _unitOfWork.SaveChangesAsync();

                    // Commit transaction nếu tất cả thao tác thành công
                    await _unitOfWork.CommitAsync();

                    return ResponseResult.Success($"Tạo môn học {subjectDetail.SubjectId} thành công và tự động thêm điểm rỗng cho sinh viên!");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);

                    // Rollback nếu có lỗi
                    await _unitOfWork.RollbackAsync();
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
            var classes = await _classesRepository.FindByIdAsync(subjectDetail.ClassId);
            if (classes == null)
            {
                throw new ClassesException.ClassesNotFoundException(subjectDetail.ClassId);
            }
           
            var result = new SubjectDetailViewModel()
            {
                SubjectDetailId = subjectDetailId,
                Credits = subjectDetail.Credits,
                TeacherName = subjectDetail.Teacher.FullName,
                SubjectName = subjectDetail.Subject.SubjectName,
                ClassName = classes.ClassName

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
