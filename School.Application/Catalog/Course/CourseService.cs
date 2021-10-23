using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using School.Utilities.Constants;
using School.Utilities.Exceptions;
using School.ViewModels.Catalog.Course.Request;
using School.ViewModels.Catalog.Course.Respone;
using School.ViewModels.Common;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace School.Application.Catalog.Course
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public CourseService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<PagedResult<CourseViewModelResponse>> Add(CourseViewModelRequest request)
        {
            var model = _mapper.Map<School.Core.Entities.Course>(request);

            var department = await _unitOfWorks.DepartmentRepository.GetQuery().SingleOrDefaultAsync(x => x.Name == request.DepartmentName);
            if (department == null) throw new SchoolException($"The department is not exist");

            model.DepartmentId = department.Id;

            _unitOfWorks.CourseRepository.Create(model);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<CourseViewModelResponse>()
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
            var model = await _unitOfWorks.CourseRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == Id);
            if (model == null)
            {
                return new PagedResult<bool>()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDetails = null,
                    Message = "Course is not found",
                    Success = false,
                    Result = false
                };
            }

            _unitOfWorks.CourseRepository.Remove(model);
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

        public async Task<PagedResult<ListResult<CourseViewModelResponse>>> GetAllPaging(PagingCourseViewModelRequest request)
        {
            var query = _unitOfWorks.CourseRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(x => x.Title.Contains(request.Title));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CourseViewModelResponse()
                {
                    Title = x.Title,
                    Credits = x.Credits,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<CourseViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<CourseViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<CourseViewModelResponse> GetById(Guid id)
        {
            var model = await _unitOfWorks.CourseRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new CourseViewModelResponse()
            {
                Id = model.Id,
                Title = model.Title,
                Credits = model.Credits,
            };

            return respone;
        }

        public async Task<PagedResult<CourseViewModelResponse>> Update(CourseViewModelRequest request)
        {
            var model = _unitOfWorks.CourseRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (model == null) return new PagedResult<CourseViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Course is not found",
                Success = true,
                Result = null
            };

            model.Title = request.Title;
            model.Credits = request.Credits;

            _unitOfWorks.CourseRepository.Update(model);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<CourseViewModelResponse>()
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
