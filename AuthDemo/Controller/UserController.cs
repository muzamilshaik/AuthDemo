using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthDemo.Dto;
using AuthDemo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AuthDemo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration Configuration { get; set; }
        private IUserRepository userRepository { get; set; }
        public UserController(IConfiguration _configuration, IUserRepository _userRepository)
        {
            Configuration = _configuration;
            userRepository = _userRepository;
        }

        [HttpPost]
        public ResultDto<int?> CreateUser([FromBody]CreateUserDto createUserDto)
        {
            try
            {
                #region Validation

                #endregion Validation

                #region Operation

                var result = userRepository.CreateUser(createUserDto);

                #endregion Operation

                #region Result

                return new ResultDto<int?>()
                {
                    Data = result,
                    IsSuccess = true,
                    Error = string.Empty
                };

                #endregion Result
            }
            catch (Exception ex)
            {
                return new ResultDto<int?>()
                {
                    Data = null,
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        [HttpGet]
        public ResultDto<LoginDto> Login(string email, string password)
        {
            try
            {
                #region Validation

                #endregion Validation

                #region Operation

                var result = userRepository.Login(email,password);

                #endregion Operation

                #region Result

                return new ResultDto<LoginDto>()
                {
                    Data = result,
                    IsSuccess = true,
                    Error = string.Empty
                };

                #endregion Result
            }
            catch (Exception ex)
            {
                return new ResultDto<LoginDto>()
                {
                    Data = null,
                    IsSuccess =false,
                    Error = ex.Message
                };
            }
        }
    }
}
