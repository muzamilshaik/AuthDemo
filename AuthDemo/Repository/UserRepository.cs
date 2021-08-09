using AuthDemo.Context;
using AuthDemo.Dto;
using AuthDemo.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;
        private readonly IConfiguration _configuration;
        //public UserRepository()
        //{
        //    _dbContext = new UserDbContext();
        //}
        public UserRepository(IConfiguration configuration, UserDbContext userDbContext)
        {
            _dbContext = userDbContext;
            _configuration = configuration;
        }
        public IEnumerable<UserDto> GetUsers()
        {
            var users = _dbContext.UserClients.ToList();

            return users.Select(c => new UserDto()
            {
                Id = c.Id.ToString(),
                Email = c.Email
            }).ToList();
        }

        public UserDto GetUserById(int id)
        {
            var user = _dbContext.UserClients.Where(u => u.Id == id).FirstOrDefault();
            return new UserDto()
            {
                Id = user.Id.ToString(),
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                IsRemoved = false
            };
        }

        public UserDto GetUserByEmail(string email)
        {
            var user = _dbContext.UserClients.Where(u => u.Email.Equals(email)).FirstOrDefault();
            return new UserDto()
            {
                Id = user.Id.ToString(),
                Title = user.Title,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                IsRemoved = false
            };
        }

        public int CreateUser(CreateUserDto createUserDto)
        {
            var user = new UserClient()
            {
                Title = createUserDto.Title,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                Email = createUserDto.Email,
                Address = createUserDto.Address,
                PhoneNumber = createUserDto.PhoneNumber,
                Password = createUserDto.Password,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                IsRemoved = false
            };
            _dbContext.UserClients.Add(user);
            Save();

            return user.Id;
        }

        public bool UpdateUser(UpdateUserDto updateUserDto)
        {
            var user = _dbContext.UserClients.Find(Convert.ToInt32(updateUserDto.Id));

            if(user != null)
            {
                user.Title = updateUserDto.Title;
                user.FirstName = updateUserDto.FirstName;
                user.LastName = updateUserDto.LastName;
                user.Address = updateUserDto.Address;
                user.PhoneNumber = updateUserDto.PhoneNumber;
                user.UpdateDate = DateTime.UtcNow;

                _dbContext.UserClients.Update(user);
                _dbContext.Entry(user).State = EntityState.Modified;

                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool DeleteUser(int id)
        {
            var user = _dbContext.UserClients.Find(id);
            if(user != null)
            {
                user.IsRemoved = true;
                _dbContext.UserClients.Update(user);

                return true;
            }
            else
            {
                return false;
            }           
        }

        public LoginDto Login(string email, string password)
        {
            if (email != null && password != null)
            {
                var user = _dbContext.UserClients.Where(c => c.Email.Equals(email) && c.Password.Equals(password)).FirstOrDefault();
                //var user = new UserClient() { Email = email, Id = 2, FirstName = "",LastName = "" };


                if (user != null)
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Id", user.Id.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName", user.LastName),
                    new Claim("Email", user.Email)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha384);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddDays(1),
                        signingCredentials: signIn);

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return new LoginDto() { Email = email, Token = tokenString };
                }
                else
                {
                    return new LoginDto() { Email = email, Token = "Invalid email or Password"};
                }
            }
            else
            {
                return new LoginDto() { Email = email, Token = "Email or Password are required" };
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
