using App.Core.Dto;
using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        private readonly IRegisterService _registerService;
        private readonly IEmailService _emailService;

        public LoginService(AppDbContext appDbContext, IConfiguration configuration, IRegisterService registerService, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
            _registerService = registerService;
            _emailService = emailService;
        }
        public async Task addOtp(Otp otp)
        {
            if(otp != null)
            {
                //var query = "SELECT * FROM Otp WHERE userId = @userId";

                //var user = await _connection.QueryFirstOrDefaultAsync(query, new {userId = otp.userId});

                var user = await _appDbContext.Otp.FirstOrDefaultAsync(u=> u.userId == otp.userId);

                if(user == null)
                {
                    await _appDbContext.Otp.AddAsync(otp);
                    await _appDbContext.SaveChangesAsync();

                }
                else
                {
                    user.otp = otp.otp;
                    await _appDbContext.SaveChangesAsync();
                }
            }
            

        }

        public async Task<bool> forgotPassword(string email)
        {
            var user = await _registerService.getUserByEmail(email);
            if (user != null)
            {
                string emailSubject = "Login Details";
                var password =  _registerService.generatePassword();

                //var message = $"Use the following details for you login \n username:{username}\n password:{password} ";
                string message = $"Hi {user.firstName},<br><br>You have been successfully registered with us!!!<br>Your login credentials are:<br>Username:" +
                    $" <b>{user.username}<b><br>Password: <b>{password}<b><br><br>We would advice you to change your password after logging in for the very first time";
                _emailService.SendEmail(email, emailSubject, message);
                
                user.password = BCrypt.Net.BCrypt.HashPassword(password, 13); 
                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> getCartId(int userId)
        {
            var cartDetails = await _appDbContext.CartMaster.FirstOrDefaultAsync(c=> c.userId == userId);
            return cartDetails.id;
        }

        public async Task<bool> verifyOtp(OtpVerifyDto dto)
        {
            var user = await _registerService.getUserByUsername(dto.username);

            if(user != null)
            {
                var query = "SELECT * FROM Otp WHERE userId = @userId";

                var otpData = await  _connection.QueryFirstOrDefaultAsync(query, new { userId = user.id });
                if(otpData != null)
                {
                    if(dto.otp == otpData.otp)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;


        }
    }
}
