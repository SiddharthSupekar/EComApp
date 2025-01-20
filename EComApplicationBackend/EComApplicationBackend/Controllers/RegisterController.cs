using App.Core.Dto;
using App.Core.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpGet]
        public async Task<IActionResult> getUsers()
        {
           var users = await _registerService.getAllUsers();
            return Ok(users);
        }

        [HttpGet("email")]
        public async Task<IActionResult> getByEmail(string email)
        {
            if(email != null)
            {
                var user = await _registerService.getUserByEmail(email);
                if(user != null)
                {
                    return Ok(user);
                }
                return NotFound($"User by the email{email} doesnot exist ");
            }
            return BadRequest("Null object passed");
        }

        [HttpGet("roles")]
        public async Task<IActionResult> getRoles()
        {
            var roles = await _registerService.getRoles();

            return Ok(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] UserDto userDto, IFormFile? profileImage)
        {
            if (userDto != null)
            {

                if (await _registerService.getUserByEmail(userDto.email) == null)
                {
                    await _registerService.createUser(userDto, profileImage);
                    return Ok(userDto);
                }
                return BadRequest("Already Exists");
            }
            return BadRequest("Null object passed");
        }


        [HttpPut("changePassword")]
        public async Task<IActionResult> changePassword(ChangePasswordDto dto)
        {
            if(dto.username != null && dto.oldPassword != null && dto.newPassword != null )
            {
                if(await _registerService.changePassword(dto.username, dto.oldPassword, dto.newPassword))
                {
                    return Ok();
                }
                return Unauthorized("Old password is not correct");
            }
            return BadRequest("Missing username or password");
        }

        [HttpPut("update")]
        public async Task<IActionResult> put(  string username, [FromForm] UserDto userDto, IFormFile? profileImage)
        {
            if (userDto != null)
            {
                 await _registerService.updateProfile(username, userDto, profileImage);
                return Ok();
            }
            return BadRequest();
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> updateAddress(string email, string address)
        {
            if(email != null && address!= null)
            {
                var result = await _registerService.updateAddress(email, address);
                if (result)
                {
                    return Ok();
                }
                return NotFound("User by the email not found");
            }
            return BadRequest("Null entry found");
        }

    }
}
