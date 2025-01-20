using App.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IJwtTokenService
    {
        public string GenerateToken(TokenDto dto);
    }
}
