using AutoMapper;
using Domain.Entites;
using Domain.Interfaces;
using Mapping.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trade.Interfaces;

namespace Trade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUserDTO userDto)
        {
            if (await UserExist(userDto)) return BadRequest("Username is taken");

            var user = _mapper.Map<User>(userDto);

            _unitOfWork.Users.Add(user);
            _unitOfWork.Users.AddPassword(user, userDto.Password);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        private Task<bool> UserExist(RegisterUserDTO userDto)
        {
            return _unitOfWork.Users.ExistAsync(u => u.UserName == userDto.UserName);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginUserDTO userDto)
        {
            var user = await _unitOfWork.Users.GetUserByUsername(userDto.UserName);

            if (user is null) return BadRequest("Invalid Username");

            var passwordValidation = _unitOfWork.Users.PasswordValidation(user, userDto.Password);

            if (!passwordValidation) return Unauthorized("Invalid Password");

            return new UserDTO()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }
    }
}
