using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Application.Catalog.Enrollment;
using School.ViewModels.Catalog.Enrollment.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : BaseApiController
    {
        private readonly IEnrollmentService _enrollmentService;

        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EnrollmentViewModelRequest request)
        {
            var result = await _enrollmentService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] EnrollmentViewModelRequest request)
        {
            var result = await _enrollmentService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _enrollmentService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingEnrollmentViewModelRequest request)
        {
            var result = await _enrollmentService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _enrollmentService.GetById(Id);
            return Ok(result);
        }

        [HttpGet("get-info-grade")]
        public async Task<IActionResult> GetInfoOfAClass(string grade)
        {
            var result = await _enrollmentService.GetInfoOfAClass(grade);
            return Ok(result);
        }
    }
}
