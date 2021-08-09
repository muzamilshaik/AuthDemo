using AuthDemo.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace AuthDemo.Repository
{
    public interface IUserRepository
    {
        IEnumerable<UserDto> GetUsers();
        UserDto GetUserById(int id);
        int CreateUser(CreateUserDto createUserDto);
        bool UpdateUser(UpdateUserDto updateUserDto);
        bool DeleteUser(int id);
        LoginDto Login(string email,string password);
        void Save();
    }
}
