using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using School.ViewModels.Catalog.Student.Request;
using School.ViewModels.Catalog.Student.Respone;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using School.ViewModels.Common;
using School.Utilities.Constants;
using System;
using School.Core.Entities;
using School.Application.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Net;

namespace School.Application.Catalog.Student
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;
        private readonly IStorageService _storageService;

        public StudentService(IUnitOfWorks unitOfWorks, IMapper mapper, IStorageService storageService)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _storageService = storageService;
        }

        public async Task<PagedResult<StudentViewModelResponse>> Add(StudentViewModelRequest request)
        {
            var student = _mapper.Map<School.Core.Entities.Student>(request);

            //Save image
            if (request.ThumbnailImage != null)
            {
                student.StudentImages = new List<StudentImage>()
                {
                    new StudentImage()
                    {
                        Caption = "Thumbnail image",
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        SortOrder = 1
                    }
                };
            }

            _unitOfWorks.StudentRepository.Create(student);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<StudentViewModelResponse>()
            {
                ErrorCode = (int) HttpStatusCode.Created,
                ErrorDetails = null,
                Message = MessageError.AddSuccess,
                Success = true,
                Result = await this.GetById(student.Id)
            };
        }

        public async Task<PagedResult<bool>> Delete(Guid Id)
        {
            var student = _unitOfWorks.StudentRepository.GetQuery().SingleOrDefault(x => x.Id == Id);
            if (student == null)
            {
                return new PagedResult<bool>()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDetails = null,
                    Message = "Customer is not found",
                    Success = false,
                    Result = false
                };
            }

            var images = _unitOfWorks.StudentImageRepository.GetQuery().Where(x => x.StudentId == Id);

            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _unitOfWorks.StudentRepository.Remove(student);
            await _unitOfWorks.SaveAsync();

            return new PagedResult<bool>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.DeleteSuccess,
                Success = true,
                Result = true
            }; ;
        }

        public async Task<List<StudentViewModelResponse>> GetAll()
        {
            var listCustomers = await _unitOfWorks.StudentRepository.GetQuery().Select(rc => new StudentViewModelResponse()
            {
                LastName = rc.LastName,
                FirstName = rc.FirstName,
                EnrollmentDate = rc.EnrollmentDate,
            }).ToListAsync();

            return listCustomers;
        }

        public async Task<PagedResult<ListResult<StudentViewModelResponse>>> GetAllPaging(PagingStudentViewModelRequest request)
        {
            var query = _unitOfWorks.StudentRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(request.FirstName));
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                query = query.Where(x => x.LastName.Contains(request.LastName));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new StudentViewModelResponse()
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    EnrollmentDate = x.EnrollmentDate
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<StudentViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<StudentViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<StudentViewModelResponse> GetById(Guid id)
        {
            var student = await _unitOfWorks.StudentRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new StudentViewModelResponse()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate,
            };

            return respone;
        }

        public async Task<PagedResult<StudentViewModelResponse>> Update(StudentViewModelRequest request)
        {
            var student = _unitOfWorks.StudentRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (student == null) return new PagedResult<StudentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Customer is not found",
                Success = true,
                Result = null
            };

            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            student.EnrollmentDate = request.EnrollmentDate;
            student.UpdatedDate = DateTime.UtcNow;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _unitOfWorks.StudentImageRepository.GetQuery().FirstOrDefaultAsync(x => x.StudentId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _unitOfWorks.StudentImageRepository.Update(thumbnailImage);
                }
            }

            _unitOfWorks.StudentRepository.Update(student);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<StudentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.AddSuccess,
                Success = true,
                Result = await GetById(student.Id)
            };

            return respone;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<PagedResult<StudentAndListEnrollmentViewModelResponse>> GetStudentAndListEnrollment(Guid Id)
        {
            var student = await _unitOfWorks.StudentRepository.GetQuery().Include(x => x.Enrollments).FirstOrDefaultAsync(x => x.Id == Id);

            var result = new StudentAndListEnrollmentViewModelResponse()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate,
                Enrollments = student.Enrollments.Select(x => new EnrollmentList()
                {
                    Grade = x.Grade,
                    Title = _unitOfWorks.CourseRepository.GetById(x.CourseId).Title,
                    Credits = _unitOfWorks.CourseRepository.GetById(x.CourseId).Credits,
                }).ToList()
            };

            return new PagedResult<StudentAndListEnrollmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.Created,
                ErrorDetails = null,
                Message = MessageError.AddSuccess,
                Success = true,
                Result = result
            };
        }
    }
}
