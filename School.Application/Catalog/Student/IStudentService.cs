using School.ViewModels.Catalog.Student.Request;
using School.ViewModels.Catalog.Student.Respone;
using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Catalog.Student
{
    public interface IStudentService
    {
        Task<PagedResult<StudentViewModelResponse>> Add(StudentViewModelRequest request);
        Task<PagedResult<StudentViewModelResponse>> Update(StudentViewModelRequest request);
        Task<PagedResult<bool>> Delete(Guid Id);
        Task<StudentViewModelResponse> GetById(Guid id);
        Task<List<StudentViewModelResponse>> GetAll();
        Task<PagedResult<ListResult<StudentViewModelResponse>>> GetAllPaging(PagingStudentViewModelRequest request);
        Task<PagedResult<StudentAndListEnrollmentViewModelResponse>> GetStudentAndListEnrollment(Guid Id);
    }
}
