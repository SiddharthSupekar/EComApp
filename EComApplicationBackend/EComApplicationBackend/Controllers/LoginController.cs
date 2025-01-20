using App.Core.Dto;
using App.Core.Interfaces;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IRegisterService _registerService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IEmailService _emailService;
        private readonly ILoginService _loginService;

        public LoginController(IRegisterService registerService,IJwtTokenService jwtTokenService, IEmailService emailService, ILoginService loginService)
        {
            _registerService = registerService;
            _jwtTokenService = jwtTokenService;
            _emailService = emailService;
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(LoginDto dto)
        {

            if (dto != null)
            {
                var user = await _registerService.getUserByUsername(dto.username);
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(dto.password, user.password ))
                    {
                        var roleId = user.roleId;
                        var roleName = await _registerService.getRoleOfUser(user.username);

                        var tokenLogin = new TokenDto
                        {
                            email = user.email,
                            roleName = roleName,
                            id = user.id
                        };
                        var token = _jwtTokenService.GenerateToken(tokenLogin);

                        string emailSubject = "2-factorAuthentication OTP";
                        //var details = await _registerService.getUserByUsername(dto.username);

                        var firstName = user.firstName;
                        var lastName = user.lastName;
                        var username = firstName + " " + lastName;
                        var generatedOtp = _emailService.GenerateOtp();
                        Otp otp = new Otp();

                        otp.otp = generatedOtp;
                        otp.userId = user.id;

                        await _loginService.addOtp(otp);

                        var message = $"The One-Time-Password for the session is {generatedOtp}";

                        _emailService.SendEmail(user.email, emailSubject, message);
                     //   var roleName = await _registerService.getRoleOfUser(user.username);

                        if(user.roleId == 2)
                        {
                            var cartId = await _loginService.getCartId(user.id);
                            return Ok(new { token = token, roleName = roleName, username = user.username, email = user.email, userId = user.id, cartId = cartId });
                        }

                        return Ok(new { token = token, roleName = roleName,  username =   user.username, email = user.email, userId =  user.id });
                    }
                    return BadRequest("Invalid Password");
                }
                return NotFound("Not Found");
            }
            return BadRequest();
        }


        [HttpPost("otpVerify")]
        public async Task<IActionResult> validatingOtp(OtpVerifyDto dto)
        {
            if (dto == null )
            {
                return BadRequest();
            }
            if(await _registerService.getUserByUsername(dto.username) != null)
            {
                if (await _loginService.verifyOtp(dto))
                {
                    return Ok();
                }
                return BadRequest("Invalid otp ");
            }
            return NotFound("Username not found");

        }

        [HttpGet("forgotPassword")]
        public async Task<IActionResult> forgotPassword(string email)
        {
            if(email != null)
            {
                if(await _loginService.forgotPassword(email))
                {
                    return Ok();
                }
                return NotFound("Email doesnot exists");
            }
            return BadRequest();
        }
    }
}
