using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using School.Utilities.Constants;
using School.Utilities.Exceptions;
using School.ViewModels.Catalog.Course.Respone;
using School.ViewModels.Catalog.Department.Request;
using School.ViewModels.Catalog.Department.Response;
using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Catalog.Department
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public DepartmentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<PagedResult<DepartmentViewModelResponse>> Add(DepartmentViewModelRequest request)
        {
            var model = _mapper.Map<School.Core.Entities.Department>(request);

            var instructor = await _unitOfWorks.InstructorRepository.GetQuery().SingleOrDefaultAsync(x => x.LastName == request.InstructorLastName);
            if (instructor == null) throw new SchoolException($"The instructor is not exist");

            model.InstructorId = instructor.Id;

            _unitOfWorks.DepartmentRepository.Create(model);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<DepartmentViewModelResponse>()
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
            var model = await _unitOfWorks.DepartmentRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == Id);
            if (model == null)
            {
                return new PagedResult<bool>()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDetails = null,
                    Message = "Department is not found",
                    Success = false,
                    Result = false
                };
            }

            _unitOfWorks.DepartmentRepository.Remove(model);
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

        public async Task<PagedResult<ListResult<DepartmentViewModelResponse>>> GetAllPaging(PagingDepartmentViewModelRequest request)
        {
            var query = _unitOfWorks.DepartmentRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.Name))
            {
                query = query.Where(x => x.Name.Contains(request.Name));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new DepartmentViewModelResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Budget = x.Budget,
                    StartDate = x.StartDate,
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<DepartmentViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<DepartmentViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<DepartmentViewModelResponse> GetById(Guid id)
        {
            var model = await _unitOfWorks.DepartmentRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new DepartmentViewModelResponse()
            {
                Id = model.Id,
                Name = model.Name,
                Budget = model.Budget,
                StartDate = model.StartDate,
            };

            return respone;
        }

        public async Task<PagedResult<ListResult<GetGroupDepartmentViewModelResponse>>> GetDepartmentGroupCourse()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<DepartmentViewModelResponse>> Update(DepartmentViewModelRequest request)
        {
            var model = _unitOfWorks.DepartmentRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (model == null) return new PagedResult<DepartmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Department is not found",
                Success = true,
                Result = null
            };

            model.Name = request.Name;
            model.Budget = request.Budget;
            model.StartDate = request.StartDate;

            _unitOfWorks.DepartmentRepository.Update(model);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<DepartmentViewModelResponse>()
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
