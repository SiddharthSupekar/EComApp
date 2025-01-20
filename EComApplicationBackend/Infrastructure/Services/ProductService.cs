using App.Core.Dto;
using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public ProductService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
        }

        public async Task addProduct(int userId, ProductDto productDto, IFormFile? productImageFile)
        {
            Product product = new Product
            {
                userId = userId,
                productName = productDto.productName,
                productCode = productDto.productCode,
                category = productDto.category,
                brand = productDto.brand,
                sellingPrice = productDto.sellingPrice,
                purchasePrice = productDto.purchasePrice,
                stock = productDto.stock,
                purchaseDate = productDto.purchaseDate
            };

            if(productImageFile != null )
            {
                if(productImageFile.ContentType == "image/jpeg" || productImageFile.ContentType == "image/png")
                {
                    var uploadFolder = Path.Combine("wwwroot", "uploads", "productImages");
                    Directory.CreateDirectory(uploadFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImageFile.FileName;
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await productImageFile.CopyToAsync(fileStream);
                    }

                    product.productImage = Path.Combine("uploads","productImages",uniqueFileName);
                }
                else
                {
                    throw new Exception("Invalid file format. Only JPG and PNG are supported");
                }
            }
            else
            {
               product.productImage = Path.Combine("uploads", "productImages", "defaultProduct.jpg");
            }

            await _appDbContext.Products.AddAsync(product);
            await _appDbContext.SaveChangesAsync();

        }
         
        public async Task<List<Product>> getAllProducts()
        {
            var query = "SELECT * FROM PRODUCTS ";

            var products = (List<Product>) await _connection.QueryAsync<Product>(query);

            return products;
        }

        public async Task<List<Product>> getAllProductsAvailableForCustomer()
        {
            var query = "SELECT * FROM PRODUCTS WHERE isACtive = 1 ";

            var products = (List<Product>)await _connection.QueryAsync<Product>(query);

            return products;
        }

        public async Task<List<Product>> getProductsByUserId(int userId)
        {
            var query = "SELECT * FROM PRODUCTS WHERE userId = @userId AND isACtive = 1 ";

            var products = (List<Product>)await _connection.QueryAsync<Product>(query, new {userId = userId});

            return products;
        }

        public async Task<List<Product>> getAvailableProducts()
        {
            var query = "SELECT * FROM PRODUCTS WHERE STOCK>0";

            var products = (List<Product>)await _connection.QueryAsync<Product>(query);

            return products;
        }
        public async Task<Product> getProductById(string productId)
        {
            var query = "SELECT * FROM PRODUCTS WHERE ID = @id AND isActive = 'true'";

            var product = await _connection.QueryFirstOrDefaultAsync<Product>(query, new { id = productId });

            return product;
        }

        public async Task<Product> getProductByCode(string productCode)
        {
            var query = "SELECT * FROM PRODUCTS WHERE PRODUCTCODE = @productCode";
            var product = await _connection.QueryFirstOrDefaultAsync(query, new { productCode = productCode });
            if(product != null)
            {
                return product;
            }
            return null;
        }

        public async Task updateProduct(string productId, ProductDto productDto, IFormFile? productImage)
        {
            var product = await getProductById(productId);
            if(product != null)
            {
                product.productName = productDto.productName;
                product.purchasePrice = productDto.purchasePrice;
                product.brand = productDto.brand;
                product.purchaseDate = productDto.purchaseDate;
                product.sellingPrice = productDto.sellingPrice;
                product.category = productDto.category;
                product.purchasePrice = product.purchasePrice;
                product.stock = productDto.stock;

                if(productImage != null)
                {
                    if(productImage.ContentType == "image/jpeg" ||  productImage.ContentType == "image/png")
                    {
                        if (!string.IsNullOrEmpty(productImage.FileName))
                        {
                            var existingPath = Path.Combine("wwwroot", product.productImage);
                            if (File.Exists(existingPath))
                            {
                                File.Delete(existingPath);
                            }


                        }

                        var uploadPath = Path.Combine("wwwroot", "uploads", "productImages");

                        Directory.CreateDirectory(uploadPath);

                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + productImage.FileName;
                        var newFilePath = Path.Combine(uploadPath, uniqueFileName);

                        using(var fileStream = new FileStream(newFilePath, FileMode.Create))
                        {
                            await productImage.CopyToAsync(fileStream);
                        }
                        product.productImage = Path.Combine("uploads", "productImages", uniqueFileName);
                    }
                    else
                    {
                        throw new Exception("Invalid file format. Only JPG and PNG are supported.");
                    }


                }
                _appDbContext.Products.Update(product);
                await _appDbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> deleteProduct(int productId)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.id == productId);
            if (product != null)
            {
                product.isActive = false;
                _appDbContext.Products.Update(product);
                await _appDbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }




    }
}
