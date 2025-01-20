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
    public interface IProductService
    {
        public Task addProduct(int userId,ProductDto productDto, IFormFile? productImageFile);
        public Task<List<Product>> getAllProducts();
        public Task<List<Product>> getAllProductsAvailableForCustomer();
        public Task<List<Product>> getProductsByUserId(int userId);
        public Task<List<Product>> getAvailableProducts();
        public Task<Product> getProductById(string productId);
        public Task updateProduct(string productId, ProductDto productDto, IFormFile? productImageFile);
        public Task<Product> getProductByCode(string productCode);   
        public Task<bool> deleteProduct(int productId);

    }
}
