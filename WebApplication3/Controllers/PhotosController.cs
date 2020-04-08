using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleAPI.Data;
using SampleAPI.DTOs;
using SampleAPI.Helper;
using SampleAPI.Model;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SampleAPI.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(
            _cloudinaryConfig.Value.CloudName,
            _cloudinaryConfig.Value.ApiKey,
            _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var PhotoFromRepo = await _repo.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDTO>(PhotoFromRepo);
            return Ok(photo);
        }

        [HttpPost("{id}/SetMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            { return Unauthorized(); }
            var user = await _repo.GetUser(userId);

            if(!user.Photos.Any(p=> p.Id==id))
            { return Unauthorized(); }

            var photoFromRepo = await _repo.GetPhoto(id);

            if(photoFromRepo.isMain)
            {
                return BadRequest("This photo is already the main photo");
            }

            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.isMain = false;

            photoFromRepo.isMain = true;

            if(await _repo.SaveAll())
            {
                return NoContent();
            }
            return BadRequest("Could not set photo to main photo");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }
            var user = await _repo.GetUser(userId);

            if (!user.Photos.Any(p => p.Id == id))
            {
                return Unauthorized();
            }

            var photoFromRepo = await _repo.GetPhoto(id);

            if (photoFromRepo.isMain)
            {
                return BadRequest("You can not delete the main photo");
            }

            if (photoFromRepo.PublicId != null)
            {
                var DeleteParams = new DeletionParams(photoFromRepo.PublicId);
                var Results = _cloudinary.Destroy(DeleteParams);

                if (Results.Result.ToLower() == "ok")
                    _repo.Delete(photoFromRepo);

            }
            else
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delte the photo");
        }
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDTO photoForCreationDTO)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            { return Unauthorized(); }
            var userFromRepo = await _repo.GetUser(userId);
            var File = photoForCreationDTO.File;
            var uploadResult = new ImageUploadResult();

            if (File.Length > 0)
            {
                using (var Stream = File.OpenReadStream())
                {
                    var UploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(File.Name, Stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(UploadParams);
                }
            }
            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var Photo = _mapper.Map<Photo>(photoForCreationDTO);

            if (!userFromRepo.Photos.Any(u => u.isMain))
                Photo.isMain = true;

            userFromRepo.Photos.Add(Photo);

            if (await _repo.SaveAll())
            {
                var PhotoToReturn = _mapper.Map<PhotoForReturnDTO>(Photo);
                return CreatedAtRoute("GetPhoto", new { id = Photo.Id }, PhotoToReturn);
            }
            return BadRequest("Error during photo upload");
        }
    }
}
