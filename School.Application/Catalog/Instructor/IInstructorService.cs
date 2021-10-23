using School.ViewModels.Catalog.Instructor.Request;
using School.ViewModels.Catalog.Instructor.Response;
using School.ViewModels.Common;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace School.Application.Catalog.Instructor
{
    public interface IInstructorService
    {
        Task<PagedResult<InstructorViewModelResponse>> Add(InstructorViewModelRequest request);
        Task<PagedResult<InstructorViewModelResponse>> Update(InstructorViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<InstructorViewModelResponse> GetById(Guid id);
        Task<PagedResult<ListResult<InstructorViewModelResponse>>> GetAllPaging(PagingIntructorViewModelRequest request);
    }
}
