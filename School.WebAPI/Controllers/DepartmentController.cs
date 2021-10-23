using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using School.Application.Catalog.Department;
using School.ViewModels.Catalog.Department.Request;

namespace School.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DepartmentViewModelRequest request)
        {
            var result = await _departmentService.Add(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] DepartmentViewModelRequest request)
        {
            var result = await _departmentService.Update(request);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var result = await _departmentService.Delete(Id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagingDepartmentViewModelRequest request)
        {
            var result = await _departmentService.GetAllPaging(request);
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var result = await _departmentService.GetById(Id);
            return Ok(result);
        }

        [HttpGet("get-department-group-course")]
        public async Task<IActionResult> GetDepartmentGroupCourse()
        {
            var result = await _departmentService.GetDepartmentGroupCourse();
            return Ok(result);
        }
    }
}
