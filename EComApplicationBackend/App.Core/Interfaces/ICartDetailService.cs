using App.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface ICartDetailService
    {
        public Task<bool> addItemToCart(int userId, CartProductDto cartDto);
        public Task<bool> removeItemFromCart(int cartId, int productId);
        public Task<bool> reduceQuantity(int cartId, int productId);
        //public Task getCartItems(CartProductDto cartDto);
    }
}
