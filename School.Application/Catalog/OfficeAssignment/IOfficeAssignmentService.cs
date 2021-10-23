using School.ViewModels.Catalog.OfficeAssignment.Request;
using School.ViewModels.Catalog.OfficeAssignment.Response;
using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Catalog.OfficeAssignment
{
    public interface IOfficeAssignmentService
    {
        Task<PagedResult<OfficeAssignmentViewModelResponse>> Add(OfficeAssignmentViewModelRequest request);
        Task<PagedResult<OfficeAssignmentViewModelResponse>> Update(OfficeAssignmentViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<OfficeAssignmentViewModelResponse> GetById(Guid id);
        Task<PagedResult<ListResult<OfficeAssignmentViewModelResponse>>> GetAllPaging(PagingOfficeAssignmentViewModelRequest request);
    }
}
