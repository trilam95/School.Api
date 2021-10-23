using AutoMapper;
using School.Core.UnitOfWorks;
using School.ViewModels.Catalog.Instructor.Request;
using School.ViewModels.Catalog.Instructor.Response;
using School.ViewModels.Common;
using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using School.Utilities.Constants;

namespace School.Application.Catalog.Instructor
{
    public class InstructorService : IInstructorService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;

        public InstructorService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<PagedResult<InstructorViewModelResponse>> Add(InstructorViewModelRequest request)
        {
            var model = _mapper.Map<School.Core.Entities.Instructor>(request);

            _unitOfWorks.InstructorRepository.Create(model);

            await _unitOfWorks.SaveAsync();

            return new PagedResult<InstructorViewModelResponse>()
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
            var model = await _unitOfWorks.InstructorRepository.GetQuery().SingleOrDefaultAsync(x => x.Id == Id);
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

            _unitOfWorks.InstructorRepository.Remove(model);
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

        public async Task<PagedResult<ListResult<InstructorViewModelResponse>>> GetAllPaging(PagingIntructorViewModelRequest request)
        {
            var query = _unitOfWorks.InstructorRepository.GetQuery();

            //2. filter
            if (!string.IsNullOrEmpty(request.LastName))
            {
                query = query.Where(x => x.LastName.Contains(request.LastName));
            }

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query = query.Where(x => x.FirstName.Contains(request.FirstName));
            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.OrderByDescending(d => d.CreatedDate).Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new InstructorViewModelResponse()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    HireDate = x.HireDate
                }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ListResult<InstructorViewModelResponse>>()
            {
                ErrorCode = (int)HttpStatusCode.OK,
                ErrorDetails = null,
                Message = MessageError.GetDataSuccess,
                Success = true,
                Result = new ListResult<InstructorViewModelResponse>()
                {
                    TotalRecords = totalRow,
                    PageSize = request.PageSize,
                    PageIndex = request.PageIndex,
                    Items = data
                },
            };

            return pagedResult;
        }

        public async Task<InstructorViewModelResponse> GetById(Guid id)
        {
            var model = await _unitOfWorks.InstructorRepository.GetQuery().FirstOrDefaultAsync(x => x.Id == id);

            var respone = new InstructorViewModelResponse()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                HireDate = model.HireDate
            };

            return respone;
        }

        public async Task<PagedResult<InstructorViewModelResponse>> Update(InstructorViewModelRequest request)
        {
            var model = _unitOfWorks.InstructorRepository.GetQuery().SingleOrDefault(x => x.Id == request.Id);
            if (model == null) return new PagedResult<InstructorViewModelResponse>()
            {
                ErrorCode = (int)HttpStatusCode.NotFound,
                ErrorDetails = null,
                Message = "Instrutor is not found",
                Success = true,
                Result = null
            };

            model.FirstName = request.FirstName;
            model.LastName = request.LastName;
            model.HireDate = request.HireDate;

            _unitOfWorks.InstructorRepository.Update(model);
            await _unitOfWorks.SaveAsync();

            var respone = new PagedResult<InstructorViewModelResponse>()
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
