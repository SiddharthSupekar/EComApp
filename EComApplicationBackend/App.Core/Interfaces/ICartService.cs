using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface ICartService
    {
        public Task<List<dynamic>> getCartItems(int cartId);
    }
}
