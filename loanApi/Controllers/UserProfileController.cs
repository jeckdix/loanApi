using AutoMapper;
using loanApi.Dtos;
using loanApi.Models;
using loanApi.Services.UserProfileService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace loanApi.Controllers
{
    [Route("api/profile")]
    [Authorize]
    [ApiController]
    public class UserProfileController : ControllerBase
    {   
        private readonly IMapper _mapper;
        private readonly IUserProfile _userProfileService;

        public UserProfileController(IMapper mapper, IUserProfile userProfileService)
        {
            _mapper = mapper;
            _userProfileService = userProfileService;
        }

        [HttpGet("{profileId}")]
        public async Task<IActionResult> GetUserProfile(int profileId)
        {
            var profile = await _userProfileService.GetUserProfile(profileId);
            
            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateProfile(UserProfilePostDto profileNew)
        //{
        //    if (profileNew == null)
        //        return BadRequest();

        //    if(!ModelState.IsValid)
        //        return BadRequest();

        //    try
        //    {
        //        var profileMap = _mapper.Map<UserProfile>(profileNew);
        //        var profileCreated = await _userProfileService.CreateProfile(profileMap);

        //        if (!profileCreated)
        //            return StatusCode(500, "Something went wrong when creating profile");

        //        return Ok("Profile created sucessfully");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500, "Something went wrong when creating profile");
        //    }
        //}

        [HttpPut("{profileId}")]
        public async Task<IActionResult> UpdateProfile(int profileId, UserProfilePostDto request)
        {
            var profileExists = await _userProfileService.GetUserProfile(profileId);
            if (profileExists == null)
                return BadRequest("Profile does not exist");

            try
            {
                var profileMap = _mapper.Map<UserProfile>(request);
                var profileUpdated = await _userProfileService.UpdateProfile(profileId, profileMap);

                if (!profileUpdated)
                    return StatusCode(500, "Something wrong happened when creating the profile");

                return Ok(request);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something wrong happened");
            }
        }
    }
}
