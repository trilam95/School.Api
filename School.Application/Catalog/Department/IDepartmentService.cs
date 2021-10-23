using School.ViewModels.Catalog.Department.Request;
using School.ViewModels.Catalog.Department.Response;
using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Catalog.Department
{
    public interface IDepartmentService
    {
        Task<PagedResult<DepartmentViewModelResponse>> Add(DepartmentViewModelRequest request);
        Task<PagedResult<DepartmentViewModelResponse>> Update(DepartmentViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<DepartmentViewModelResponse> GetById(Guid id);
        Task<PagedResult<ListResult<DepartmentViewModelResponse>>> GetAllPaging(PagingDepartmentViewModelRequest request);
        Task<PagedResult<ListResult<GetGroupDepartmentViewModelResponse>>> GetDepartmentGroupCourse();
    }
}
