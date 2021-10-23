using School.ViewModels.Catalog.Course.Request;
using School.ViewModels.Catalog.Course.Respone;
using School.ViewModels.Common;
using System;
using System.Threading.Tasks;

namespace School.Application.Catalog.Course
{
    public interface ICourseService
    {
        Task<PagedResult<CourseViewModelResponse>> Add(CourseViewModelRequest request);
        Task<PagedResult<CourseViewModelResponse>> Update(CourseViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<CourseViewModelResponse> GetById(Guid id);
        Task<PagedResult<ListResult<CourseViewModelResponse>>> GetAllPaging(PagingCourseViewModelRequest request);
    }
}
