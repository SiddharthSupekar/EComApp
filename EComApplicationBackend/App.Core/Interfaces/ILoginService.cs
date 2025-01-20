using App.Core.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface ILoginService
    {
        public Task addOtp(Otp otp);
        public Task<bool> verifyOtp(OtpVerifyDto dto);
        public Task<bool> forgotPassword(string email);
        public Task<int> getCartId(int userId);
    }
}
