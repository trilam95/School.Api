using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Application.Catalog.Course;
using School.ViewModels.Catalog.Course.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : BaseApiController
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CourseViewModelRequest request)
        {
            var result = await _courseService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] CourseViewModelRequest request)
        {
            var result = await _courseService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _courseService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingCourseViewModelRequest request)
        {
            var result = await _courseService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _courseService.GetById(Id);
            return Ok(result);
        }
    }
}
