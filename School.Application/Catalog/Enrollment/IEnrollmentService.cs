using School.ViewModels.Catalog.Enrollment.Request;
using School.ViewModels.Catalog.Enrollment.Response;
using School.ViewModels.Common;
using System;
using System.Threading.Tasks;

namespace School.Application.Catalog.Enrollment
{
    public interface IEnrollmentService
    {
        Task<PagedResult<EnrollmentViewModelResponse>> Add(EnrollmentViewModelRequest request);
        Task<PagedResult<EnrollmentViewModelResponse>> Update(EnrollmentViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<EnrollmentViewModelResponse> GetById(Guid id);
        Task<PagedResult<ListResult<EnrollmentViewModelResponse>>> GetAllPaging(PagingEnrollmentViewModelRequest request);
        Task<PagedResult<GetInfoOfClassViewModelResponse>> GetInfoOfAClass(string grade);
    }
}
