using School.Core.Entities;
using School.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace School.Application.System.Users
{
    public interface IUserService
    {
        Task<AuthenticateModelRespone> Authenticate(AuthenticateModel request);
        Task<bool> Register(RegisterModel request);

        Task<User> GetByUserName(string UserName);
    }
}
