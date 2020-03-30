using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleAPI.Data;
using SampleAPI.DTOs;
using SampleAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var Users = await _repo.GetUsers();
            var UsersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(Users);
            return Ok(UsersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var User = await _repo.GetUser(Id);
            var UserToReturn = _mapper.Map<UserForDetailedDTO>(User);
            return Ok(UserToReturn);
        }
        //[HttpPost("register")]
        //// Post: /<controller>/
        //public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        //{
        //    userForRegisterDTO.username = userForRegisterDTO.username.ToLower();
        //    if (await _repo.UserExists(userForRegisterDTO.username))
        //     return BadRequest("User already exists, please choose different name");


        //    var UserToCreate = new User()
        //    {
        //        Username = userForRegisterDTO.username
        //    };

        //    var CreatedUser = _repo.Register(UserToCreate, userForRegisterDTO.password);
        //    return StatusCode(201);
        //}

        //[HttpPost("login")]
        //// POST: /<controller>/
        //public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        //{
        //    var UserFromRepo = _repo.Login(userForLoginDTO.username.ToLower(), userForLoginDTO.password);

        //    if (UserFromRepo.Result == null)
        //        return Unauthorized();

        //    var Claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
        //        new Claim(ClaimTypes.Name,UserFromRepo.Id.ToString())
        //    };

        //    var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value));

        //    var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

        //    var TokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(Claims),
        //        Expires = DateTime.Now.AddDays(1),
        //        SigningCredentials = Creds
        //    };

        //    var TokenHandler = new JwtSecurityTokenHandler();

        //    var Token = TokenHandler.CreateToken(TokenDescriptor);

        //    return Ok(new
        //    {
        //        Token=TokenHandler.WriteToken(Token)
        //    });
        //}
    }
}
