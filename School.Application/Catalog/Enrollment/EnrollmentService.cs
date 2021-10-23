using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using School.Utilities.Constants;
using School.Utilities.Exceptions;
using School.ViewModels.Catalog.Enrollment.Request;
using School.ViewModels.Catalog.Enrollment.Response;
using School.ViewModels.Common;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Application.Catalog.Enrollment
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public EnrollmentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }
        public async Task<PagedResult<EnrollmentViewModelResponse>> Add(EnrollmentViewModelRequest request)
        {
            var model = _mapper.Map<School.Core.Entities.Enrollment>(request);

            var course = await _unitOfWorks.CourseRepository.GetQuery().SingleOrDefaultAsync(x => x.Title == request.CourseName);
            if (course == null) throw new SchoolException($"The course is not exist");

            var student = await _unitOfWorks.StudentRepository.GetQuery().SingleOrDefaultAsync(x => x.LastName == request.StudentLastName);
            if (student == null) throw new SchoolException($"The student is not exist");

            model.CourseId = course.Id;
            model.StudentId = student.Id;

            _unitOfWorks.EnrollmentRepository.Create(model);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<EnrollmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.Created,
                ErrorDetails = null,
                Message = MessageError.AddSuccess,
                Success = true,
                Result = await this.GetById(model.Id)
            };
        }

        public async Task<PagedResult<bool>> Delete(Guid Id)
        {
            var model = await _unitOfWorks.EnrollmentRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == Id);
            if (model == null)
            {
                return new PagedResult<bool>()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDetails = null,
                    Message = "Enrollment is not found",
                    Success = false,
                    Result = false
                };
            }

            _unitOfWorks.EnrollmentRepository.Remove(model);
            await _unitOfWorks.SaveAsync();

            return new PagedResult<bool>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.DeleteSuccess,
                Success = true,
                Result = true
            };
        }

        public async Task<PagedResult<ListResult<EnrollmentViewModelResponse>>> GetAllPaging(PagingEnrollmentViewModelRequest request)
        {
            var query = _unitOfWorks.EnrollmentRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.Grade))
            {
                query = query.Where(x => x.Grade.Contains(request.Grade));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new EnrollmentViewModelResponse()
                {
                    Id = x.Id,
                    Grade = x.Grade
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<EnrollmentViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<EnrollmentViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<EnrollmentViewModelResponse> GetById(Guid id)
        {
            var model = await _unitOfWorks.EnrollmentRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new EnrollmentViewModelResponse()
            {
                Id = model.Id,
                Grade = model.Grade,
            };

            return respone;
        }

        public async Task<PagedResult<GetInfoOfClassViewModelResponse>> GetInfoOfAClass(string grade)
        {
            var model = await _unitOfWorks.EnrollmentRepository.GetQuery().Include(x => x.Course).Include(y => y.Student).Where(x => x.Grade == grade).ToListAsync();

            if (model == null) throw new SchoolException($"This " + grade + "is not exist");

            var result = new GetInfoOfClassViewModelResponse()
            {
                Grade = grade,
                InfoEnrollment = model.Select(x => new StudentAndCourseInfo() 
                {
                    FullName = x.Student.LastName + " " + x.Student.FirstName,
                    Title = x.Course.Title,
                }).ToList()
            };

            return new PagedResult<GetInfoOfClassViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = result
            };
        }

        public async Task<PagedResult<EnrollmentViewModelResponse>> Update(EnrollmentViewModelRequest request)
        {
            var model = _unitOfWorks.EnrollmentRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (model == null) return new PagedResult<EnrollmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Enrollment is not found",
                Success = true,
                Result = null
            };

            model.Grade = request.Grade;

            _unitOfWorks.EnrollmentRepository.Update(model);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<EnrollmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.UpdateSuccess,
                Success = true,
                Result = await GetById(model.Id)
            };

            return respone;
        }
    }
}
