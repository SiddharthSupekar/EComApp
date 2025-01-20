using App.Core.Dto;
using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly AppDbContext _appDbContext;
        private readonly SqlConnection _connection;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public RegisterService(AppDbContext appDbContext, IConfiguration configuration, IEmailService emailService)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
            _emailService = emailService;
        }

        public async Task<List<User>> getAllUsers()
        {
            var query = " SELECT * FROM USERS WHERE isActive = @isActive";
            var users = (List<User>)await _connection.QueryAsync<User>(query, new {isActive = true});
            //if (users != null)
            //{
            //    return users;
            //}
            //return null;
            return users;
            
        }

        public async Task<User> getUserByEmail(string email)
        {
            var query = "SELECT * FROM USERS WHERE EMAIL = @EMAIL";
            var  user = await _connection.QueryFirstOrDefaultAsync<User>(query,new { EMAIL = email });
            if(user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> getUserById(int userId)
        {
            var query = "SELECT * FROM USERS WHERE id = @userId";
            var user = await _connection.QueryFirstOrDefaultAsync<User>(query, new { userId = userId });
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<User> getUserByUsername(string username)
        {
            var query = "SELECT * FROM USERS WHERE USERNAME = @USERNAME";

            var user = await _connection.QueryFirstOrDefaultAsync<User>(query, new { USERNAME = username });

            //var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.username == username);

            if (user != null)
            {
                return user;
            }
            return null;

        }

        //public async Task createUser(UserDto userDto)
        //{

        //    User user = new User();

        //    user.firstName = userDto.firstName;
        //    user.lastName = userDto.lastName;
        //    user.email = userDto.email;
        //    user.mobile = userDto.mobile;
        //    user.roleId = userDto.roleId;
        //    user.dob = userDto.dob;
        //    user.address = userDto.address;
        //    user.countryId = userDto.countryId;
        //    user.stateId = userDto.stateId;
        //    user.zipcode = userDto.zipcode;
        //    user.profileImage = userDto.profileImage;

        //    var username = $"EC_{userDto.lastName.ToUpper()}{userDto.firstName.Substring(0, 1).ToUpper()}{userDto.dob.ToString("ddMMyy")}";
        //    var password = generatePassword();
        //    user.username = username;
        //    user.password = BCrypt.Net.BCrypt.HashPassword(password,13);

        //    await _appDbContext.Users.AddAsync(user);

        //    await _appDbContext.SaveChangesAsync();

        //    string emailSubject = "Login Details";

        //    //var message = $"Use the following details for you login \n username:{username}\n password:{password} ";
        //    string message = $"Hi {userDto.firstName},<br><br>You have been successfully registered with us!!!<br>Your login credentials are:<br>Username:" +
        //        $" <b>{username}<b><br>Password: <b>{password}<b><br><br>We would advice you to change your password after logging in for the very first time";
        //    _emailService.SendEmail(userDto.email, emailSubject, message);
        //}

        public async Task createUser(UserDto userDto, IFormFile? profileImageFile)
        {
            User user = new User();

   
            user.firstName = userDto.firstName;
            user.lastName = userDto.lastName;
            user.email = userDto.email;
            user.mobile = userDto.mobile;
            user.roleId = userDto.roleId;
            user.dob = userDto.dob;
            user.address = userDto.address;
            user.countryId = userDto.countryId;
            user.stateId = userDto.stateId;
            user.zipcode = userDto.zipcode;

            if (profileImageFile != null)
            {
                if (profileImageFile.ContentType == "image/jpeg" || profileImageFile.ContentType == "image/png")
                {
                    var uploadsFolder = Path.Combine("wwwroot", "uploads", "profileImages");
                    Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImageFile.CopyToAsync(fileStream);
                    }

                    user.profileImage = Path.Combine("upload", "profileImages", uniqueFileName);
                }
                else
                {
                    throw new Exception("Invalid file format. Only JPG and PNG are supported.");
                }
            }
            else
            {
                user.profileImage = Path.Combine( "uploads", "profileImages","defaultPic.jpg");
               

            }

 
            var username = $"EC_{userDto.lastName.ToUpper()}{userDto.firstName.Substring(0, 1).ToUpper()}{userDto.dob:ddMMyy}";
            var password = generatePassword();
            user.username = username;
            user.password = BCrypt.Net.BCrypt.HashPassword(password, 13);

            await _appDbContext.Users.AddAsync(user);
            await _appDbContext.SaveChangesAsync();
            var existingCart = await _appDbContext.CartMaster.Where(c => c.userId == user.id).FirstOrDefaultAsync();

            if(user.roleId == 2)
            {
                if (existingCart == null)
                {
                    var cart = new CartMaster
                    {
                        userId = user.id
                    };
                    await _appDbContext.CartMaster.AddAsync(cart);
                }
            }
            await _appDbContext.SaveChangesAsync();

            string emailSubject = "Login Details";
            string message = $"Hi {userDto.firstName},<br><br>You have been successfully registered with us!!!<br>Your login credentials are:<br>Username:" +
                $" <b>{username}</b><br>Password: <b>{password}</b><br><br>We advise you to change your password after logging in for the first time.";
            _emailService.SendEmail(userDto.email, emailSubject, message);
        }

        public async Task<List<Role>> getRoles()
        {
            var query = "SELECT * FROM ROLES";

            var roles =  (List<Role>) await _connection.QueryAsync<Role>(query);

            return roles;
        }

        public async Task updateProfile(string username, UserDto userDto, IFormFile? profileImageFile)
        {
            var user = await getUserByUsername(username);



            if ((await getUserByEmail(userDto.email) == null) || (user.email == userDto.email))
            {
                user.firstName = userDto.firstName;
                user.lastName = userDto.lastName;
                user.email = userDto.email;
                user.mobile = userDto.mobile;
                user.dob = userDto.dob;
                user.address = userDto.address;
                user.countryId = userDto.countryId;
                user.stateId = userDto.stateId;
                user.zipcode = userDto.zipcode;

                if (profileImageFile != null)
                {
                    if (profileImageFile.ContentType == "image/jpeg" || profileImageFile.ContentType == "image/png")
                    {
                        if (!string.IsNullOrEmpty(user.profileImage))
                        {
                            var existingPath = Path.Combine("wwwroot", user.profileImage);
                            if (File.Exists(existingPath))
                            {
                                File.Delete(existingPath);
                            }


                        }

                        var uploadPath = Path.Combine("wwwroot", "uploads", "profileImages");
                        Directory.CreateDirectory(uploadPath);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + profileImageFile.FileName;
                        var newFilePath = Path.Combine(uploadPath, uniqueFileName);

                        using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await profileImageFile.CopyToAsync(fileStream);
                        }

                        user.profileImage = Path.Combine("uploads", "profileImages", uniqueFileName);
                    }

                    else
                    {
                        throw new Exception("Invalid file format. Only JPG and PNG are supported.");
                    }


                }
                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();

            }
        }

        public async Task<bool> changePassword(string username, string oldPassword, string newPassword)
        {
            var user = await getUserByUsername(username);

            if (BCrypt.Net.BCrypt.Verify(oldPassword, user.password))
            {
                user.password = BCrypt.Net.BCrypt.HashPassword(newPassword, 13);

                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<string> getRoleOfUser(string username)
        {
            var user = await getUserByUsername(username);

            var role  =await _appDbContext.Roles.Where(r => r.roleId == user.roleId).FirstOrDefaultAsync();
            return role.roleName.ToString();

        }

        public async Task<bool> updateAddress(string email, string address)
        {
            var user = await getUserByEmail(email);
            if(user != null)
            {
                user.address = address;

                _appDbContext.Users.Update(user);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;   

        }

        public string generatePassword()
        {
            int i = 8;
            const string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (i > 0)
            {
                password.Append(characters[random.Next(characters.Length)]);
                i--;
            }
            return password.ToString();
        }

    }




}
