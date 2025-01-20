using App.Core.Dto;
using App.Core.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartDetailService : ICartDetailService
    {
        private readonly AppDbContext _appDbContext;
        public CartDetailService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<bool> addItemToCart(int userId, CartProductDto cartDto)
        {
            //var existingCart = await _appDbContext.CartMaster.Where(c=> c.userId == userId).FirstOrDefaultAsync();  
            
            //if(existingCart == null)
            //{
            //    var cart = new CartMaster
            //    {
            //        userId = userId,
            //    };
            //    await _appDbContext.CartMaster.AddAsync(cart);
            //    await _appDbContext.SaveChangesAsync();
                var existingProduct = await _appDbContext.CartDetails.Where(e => e.cartId == cartDto.cartId && e.productId == cartDto.productId).FirstOrDefaultAsync();
                if (existingProduct == null)
                {
                    var cartProduct = new CartDetail
                    {
                        cartId = cartDto.cartId,
                        productId = cartDto.productId,
                        quantity = cartDto.quantity,
                    };
                    await _appDbContext.CartDetails.AddAsync(cartProduct);
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    //var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.id == userId);
                    var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.id == cartDto.productId);
                    if (product.stock == existingProduct.quantity)
                    {
                        _appDbContext.SaveChangesAsync();
                        return false;
                    }
                    existingProduct.quantity = existingProduct.quantity + 1;
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
            //}
            //else
            //{
            //    var existingProduct = await _appDbContext.CartDetails.Where(e => e.cartId == cartDto.cartId && e.productId == cartDto.productId).FirstOrDefaultAsync();
            //    if (existingProduct == null)
            //    {
            //        var cartProduct = new CartDetail
            //        {
            //            cartId = cartDto.cartId,
            //            productId = cartDto.productId,
            //            quantity = cartDto.quantity,
            //        };
            //        await _appDbContext.CartDetails.AddAsync(cartProduct);
            //        await _appDbContext.SaveChangesAsync();
            //    }
            //    else
            //    {
            //        existingProduct.quantity = existingProduct.quantity + 1;
            //        await _appDbContext.SaveChangesAsync();
            //    }
            //}
        }
        public async Task<bool> reduceQuantity(int cartId, int productId)
        {
            //can make the return type as int and send the no. of remaining products so that in the frontend, we can know about it and it will be easier to check for out of stock
            var existing = await _appDbContext.CartDetails.Where(c => c.cartId == cartId && c.productId == productId).FirstOrDefaultAsync();

            if(existing.quantity == 1)
            {
                var result = await removeItemFromCart(cartId,productId);
                return result;
            }
            if(existing == null)
            {
                return false;
            }
            else
            {
                existing.quantity -= 1;
                await _appDbContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> removeItemFromCart(int cartId, int productId)
        {
            var existing = await _appDbContext.CartDetails.Where(c => c.cartId == cartId && c.productId == productId).FirstOrDefaultAsync();

            if (existing == null)
            {
                return false;
            }
            else
            {
                _appDbContext.CartDetails.Remove(existing);
                await _appDbContext.SaveChangesAsync();
                return true;
            }

        }
    }
}
