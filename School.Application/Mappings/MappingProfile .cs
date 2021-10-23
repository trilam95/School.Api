using AutoMapper;
using School.Core.Entities;
using School.ViewModels.Catalog.ChatMessage.Request;
using School.ViewModels.Catalog.Course.Request;
using School.ViewModels.Catalog.Department.Request;
using School.ViewModels.Catalog.Enrollment.Request;
using School.ViewModels.Catalog.Instructor.Request;
using School.ViewModels.Catalog.OfficeAssignment.Request;
using School.ViewModels.Catalog.Student.Request;
using School.ViewModels.Catalog.Student.Respone;
using School.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace School.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Entity to ViewModel
            CreateMap<User, RegisterModel>();
            CreateMap<Student, StudentViewModelRequest>();
            CreateMap<Course, CourseViewModelRequest>();
            CreateMap<Instructor, InstructorViewModelRequest>();
            CreateMap<Department, DepartmentViewModelRequest>();
            CreateMap<Enrollment, EnrollmentViewModelRequest>();
            CreateMap<OfficeAssignment, OfficeAssignmentViewModelRequest>();

            //ViewModel to Entity
            CreateMap<RegisterModel, User>();
            CreateMap<StudentViewModelRequest, Student>();
            CreateMap<CourseViewModelRequest, Course>();
            CreateMap<InstructorViewModelRequest, Instructor>();
            CreateMap<DepartmentViewModelRequest, Department>();
            CreateMap<EnrollmentViewModelRequest, Enrollment>();
            CreateMap<OfficeAssignmentViewModelRequest, OfficeAssignment>();
        }
    }
}
