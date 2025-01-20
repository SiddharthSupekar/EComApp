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
    public class CartService : ICartService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;
        public CartService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
        }

        public async Task<List<dynamic>> getCartItems(int cartId)
        {

            var query = " SELECT\tProducts.id as productId, productImage, productName, brand, sellingPrice, quantity FROM  Products LEFT JOIN CartDetails ON CartDetails.productId = Products.ID LEFT JOIN CartMaster ON CartMaster.id = CartDetails.cartId WHERE CartDetails.quantity>0 AND CARTID = @cartId";
            var cartItems = await _connection.QueryAsync(query, new {cartId = cartId});
            return cartItems.ToList();
        }
    }
}
