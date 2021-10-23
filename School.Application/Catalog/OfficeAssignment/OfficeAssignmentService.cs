using AutoMapper;
using Microsoft.EntityFrameworkCore;
using School.Core.UnitOfWorks;
using School.Utilities.Constants;
using School.Utilities.Exceptions;
using School.ViewModels.Catalog.OfficeAssignment.Request;
using School.ViewModels.Catalog.OfficeAssignment.Response;
using School.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.Catalog.OfficeAssignment
{
    public class OfficeAssignmentService : IOfficeAssignmentService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public OfficeAssignmentService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<PagedResult<OfficeAssignmentViewModelResponse>> Add(OfficeAssignmentViewModelRequest request)
        {
            var model = _mapper.Map<School.Core.Entities.OfficeAssignment>(request);

            var instructor = await _unitOfWorks.InstructorRepository.GetQuery().SingleOrDefaultAsync(x => x.LastName == request.InstructorLastName);
            if (instructor == null) throw new SchoolException($"The instructor is not exist");

            model.InstructorId = instructor.Id;

            _unitOfWorks.OfficeAssignmentRepository.Create(model);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<OfficeAssignmentViewModelResponse>()
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
            var model = await _unitOfWorks.OfficeAssignmentRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == Id);
            if (model == null)
            {
                return new PagedResult<bool>()
                {
                    ErrorCode = (int)HttpStatusCode.NotFound,
                    ErrorDetails = null,
                    Message = "Office Assignment is not found",
                    Success = false,
                    Result = false
                };
            }

            _unitOfWorks.OfficeAssignmentRepository.Remove(model);
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

        public async Task<PagedResult<ListResult<OfficeAssignmentViewModelResponse>>> GetAllPaging(PagingOfficeAssignmentViewModelRequest request)
        {
            var query = _unitOfWorks.OfficeAssignmentRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.Location))
            {
                query = query.Where(x => x.Location.Contains(request.Location));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new OfficeAssignmentViewModelResponse()
                {
                    Id = x.Id,
                    Location = x.Location
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<OfficeAssignmentViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<OfficeAssignmentViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<OfficeAssignmentViewModelResponse> GetById(Guid id)
        {
            var model = await _unitOfWorks.OfficeAssignmentRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new OfficeAssignmentViewModelResponse()
            {
                Id = model.Id,
                Location = model.Location
            };

            return respone;
        }

        public async Task<PagedResult<OfficeAssignmentViewModelResponse>> Update(OfficeAssignmentViewModelRequest request)
        {
            var model = _unitOfWorks.OfficeAssignmentRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (model == null) return new PagedResult<OfficeAssignmentViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Office Assignment is not found",
                Success = true,
                Result = null
            };

            model.Location = request.Location;

            _unitOfWorks.OfficeAssignmentRepository.Update(model);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<OfficeAssignmentViewModelResponse>()
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
