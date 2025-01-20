using App.Core.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IRegisterService
    {
        public Task createUser(UserDto userDto, IFormFile? profileImageFile);
        public Task<List<User>> getAllUsers();
        public Task<User> getUserByEmail(string email);
        public Task<User> getUserByUsername(string username);
        public Task<User> getUserById(int userId);
        public Task<bool> changePassword(string username, string oldPassword, string newPassword);
        public Task updateProfile(string username, UserDto userDto, IFormFile? profileImageFile);
        public Task<string> getRoleOfUser(string username);
        public Task<List<Role>> getRoles();
        public Task<bool> updateAddress(string email, string address);
        public string generatePassword();
    }
}
