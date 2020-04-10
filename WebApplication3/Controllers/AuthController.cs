using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleAPI.Data;
using SampleAPI.DTOs;
using SampleAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config,IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        // Post: /<controller>/
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegisterDTO)
        {
            userForRegisterDTO.username = userForRegisterDTO.username.ToLower();
            if (await _repo.UserExists(userForRegisterDTO.username))
             return BadRequest("User already exists, please choose different name");


            var UserToCreate = _mapper.Map<User>(userForRegisterDTO);
                               //commenting below part as now it will be done by automapper.
                //new User()
                //{
                //    Username = userForRegisterDTO.username
                //};

            var CreatedUser = _repo.Register(UserToCreate, userForRegisterDTO.password);
            try
            {
                var UserToReturn = new UserForDetailedDTO()
                {
                    Username = CreatedUser.Result.Username,
                    Id = CreatedUser.Result.Id,
                    Gender = CreatedUser.Result.Gender,
                    KnownAs = CreatedUser.Result.KnownAs,
                    City = CreatedUser.Result.City,
                    Country = CreatedUser.Result.Country,
                    Interests = CreatedUser.Result.Interests,
                    Introduction = CreatedUser.Result.Introduction


                }; //_mapper.Map<UserForRegisterDTO>(CreatedUser);
           
            //return StatusCode(201);
            return CreatedAtRoute("GetUser", new { Controller = "Users", id = CreatedUser.Result.Id }, UserToReturn);
            }
            catch (Exception ex)
            { throw ex; }
        }

        [HttpPost("login")]
        // POST: /<controller>/
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO)
        {
            var UserFromRepo =  await _repo.Login(userForLoginDTO.username.ToLower(), userForLoginDTO.password);

            if (UserFromRepo == null)
                return Unauthorized();

            var Claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UserFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,UserFromRepo.Username.ToString())
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Appsettings:Token").Value));

            var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = Creds
            };

            var TokenHandler = new JwtSecurityTokenHandler();

            var Token = TokenHandler.CreateToken(TokenDescriptor);
            var User = _mapper.Map<UserForListDTO>(UserFromRepo);

            return Ok(new
            {
                Token=TokenHandler.WriteToken(Token), 
                User
            });
        }
    }
}
