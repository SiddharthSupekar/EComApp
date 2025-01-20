using App.Core.Dto;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface IPaymentService
    {
        public Task<bool> verifyCard(CardDto cardDto);
    }
}
