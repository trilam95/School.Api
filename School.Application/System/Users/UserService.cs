using Microsoft.EntityFrameworkCore;
using School.Core.Entities;
using School.Core.UnitOfWorks;
using System;
using System.Threading.Tasks;
using School.ViewModels.System.Users;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using School.Utilities.Constants;
using Microsoft.Extensions.Options;
using School.Utilities.Exceptions;
using School.Application.Common;

namespace School.Application.System.Users
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWorks _unitOfWorks;
        private IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(IUnitOfWorks unitOfWorks, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _unitOfWorks = unitOfWorks;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateModelRespone> Authenticate(AuthenticateModel request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return null;

            var user = await _unitOfWorks.UserRepository.GetQuery().FirstOrDefaultAsync(x=> x.UserName == request.Username);

            if (user == null)
                return null;

            if (FuncCommon.Encrypt(request.Password, _appSettings.Key) != user.Password)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var result = new AuthenticateModelRespone()
            {
                Token = tokenHandler.WriteToken(token)
            };

            return result;
        }

        public async Task<bool> Register(RegisterModel request)
        {

            if (_unitOfWorks.UserRepository.Any(x => x.UserName == request.UserName))
                throw new SchoolException("Username \"" + request.UserName + "\" is already taken");

            request.Password = FuncCommon.Encrypt(request.Password,_appSettings.Key);

            var user = _mapper.Map<User>(request);

            _unitOfWorks.UserRepository.Create(user);
            await _unitOfWorks.SaveAsync();

            return true;
        }

        public async Task<User> GetByUserName(string UserName)
        {
            var user = await _unitOfWorks.UserRepository.GetQuery().FirstOrDefaultAsync(x => x.UserName == UserName);
            user.Password = FuncCommon.Decrypt(user.Password,_appSettings.Key);
            return user;
        }
    }
}
