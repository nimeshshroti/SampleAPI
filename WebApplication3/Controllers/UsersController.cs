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
using SampleAPI.Helper;
using SampleAPI.Helpers;
using SampleAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAPI.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery] UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser(currentUserId);
            userParams.UserId = currentUserId;
            if(string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }
            var Users = await _repo.GetUsers(userParams);
            var UsersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(Users);
            Response.AddPagination(Users.CurrentPage, Users.PageSize,Users.TotalCount,Users.TotalPages);
            return Ok(UsersToReturn);
        }

        [HttpGet("{id}",Name ="GetUser")]
        public async Task<IActionResult> GetUser(int Id)
        {
            var User = await _repo.GetUser(Id);
            var UserToReturn = _mapper.Map<UserForDetailedDTO>(User);
            return Ok(UserToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                { return Unauthorized(); }
            var userFromRepo = await _repo.GetUser(id);
            _mapper.Map(userForUpdateDTO, userFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating user {id} failed on Save.");
        }


    }
}
