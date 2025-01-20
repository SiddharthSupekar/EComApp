using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Dto
{
    public class ChangePasswordDto
    {
        public string? username { get; set; }
        public string? oldPassword { get; set; }
        public string? newPassword { get; set; }
    }
}
