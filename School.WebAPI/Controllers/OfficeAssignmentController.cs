using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Application.Catalog.OfficeAssignment;
using School.ViewModels.Catalog.OfficeAssignment.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeAssignmentController : BaseApiController
    {
        private readonly IOfficeAssignmentService _officeAssignmentService;

        public OfficeAssignmentController(IOfficeAssignmentService officeAssignmentService)
        {
            _officeAssignmentService = officeAssignmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OfficeAssignmentViewModelRequest request)
        {
            var result = await _officeAssignmentService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] OfficeAssignmentViewModelRequest request)
        {
            var result = await _officeAssignmentService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _officeAssignmentService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingOfficeAssignmentViewModelRequest request)
        {
            var result = await _officeAssignmentService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _officeAssignmentService.GetById(Id);
            return Ok(result);
        }
    }
}
