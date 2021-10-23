using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Application.Catalog.Instructor;
using School.ViewModels.Catalog.Instructor.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorController : BaseApiController
    {
        private readonly IInstructorService _intructorService;
        public InstructorController(IInstructorService intructorService)
        {
            _intructorService = intructorService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InstructorViewModelRequest request)
        {
            var result = await _intructorService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] InstructorViewModelRequest request)
        {
            var result = await _intructorService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _intructorService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingIntructorViewModelRequest request)
        {
            var result = await _intructorService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _intructorService.GetById(Id);
            return Ok(result);
        }
    }
}
