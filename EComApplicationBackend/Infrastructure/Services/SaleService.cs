using App.Core.Dto;
using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SaleService :ISaleService
    {
        private readonly AppDbContext _appDbContext;
        private readonly SqlConnection _connection;
        public SaleService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
        }

        public async Task populateSalesMasterAndSalesDetails(InvoiceDto invoiceDto)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u=> u.id == invoiceDto.userId);

            var cart = await _appDbContext.CartMaster.FirstOrDefaultAsync(c=> c.userId == invoiceDto.userId);

            var cartDetail = await _appDbContext.CartDetails.Where(c=> c.cartId == cart.id).ToListAsync();

            var invoiceId = generateInvoiceId();
            var salesMaster = new SalesMaster
            {
                invoiceId = invoiceId,
                userId = user.id,
                subTotal = invoiceDto.subTotal,
                invoiceDate = DateTime.Now,
                deliveryAddress = user.address,
                deliveryZipcode = user.zipcode,
                deliveryCountry = user.countryId,
                deliveryState = user.stateId
            };
            foreach (var item in cartDetail)
            {
                var product = await _appDbContext.Products.FirstOrDefaultAsync(p => p.id == item.productId);
          
                var salesDetails = new SalesDetail
                {
                    productId = item.productId,
                    invoiceId = invoiceId,
                    productCode = product.productCode,
                    saleQuantity = item.quantity,
                    sellingPrice = product.sellingPrice
                };
                product.stock = product.stock - item.quantity;
                await _appDbContext.SalesDetails.AddAsync(salesDetails);
                _appDbContext.Products.Update(product);
            }
            await _appDbContext.SalesMaster.AddAsync(salesMaster);

            await _appDbContext.SaveChangesAsync();

            _appDbContext.CartDetails.RemoveRange(cartDetail);
            await _appDbContext.SaveChangesAsync();
            

        }

        public  string generateInvoiceId()
        {
            //var query = "SELECT INVOICEID TOP(1) FROM SALESMASTER ORDER BY INVOICEID DESC";
            //var lastInvoice =  _connection.QueryFirstOrDefaultAsync<SalesMaster>(query);
            var lastInvoice= _appDbContext.SalesMaster.OrderByDescending(s=> s.invoiceId).FirstOrDefault();
            if (lastInvoice == null)
            {
                return "ORD-001";
            }

            int lastInvoiceNumber = Convert.ToInt32(lastInvoice.invoiceId.Substring(4));
            int newInvoiceNumber = lastInvoiceNumber + 1;
            return "ORD-" + newInvoiceNumber.ToString();
        }

        public async Task<ResponseInvoiceDto> generateReciept(int userId)
        {
            //var queryForSalesMaster = "SELECT INVOICEID, SUBTOTAL, INVOICEDATE, DELIVERYADDRESS, DELIVERYZIPCODE, DELIVERYSTATE, DELIVERYCOUNTRY FROM SALESMASTER WHERE USERID=@userId";
            //var data = await _connection.QueryFirstOrDefaultAsync(queryForSalesMaster, new {userId = userId});
            //var data = await _appDbContext.SalesMaster.FirstOrDefaultAsync(s => s.userId == userId);
            var data = await _appDbContext.SalesMaster.Where(s => s.userId == userId).OrderByDescending(s => s.invoiceId).FirstOrDefaultAsync();
                
            //var queryForSalesDetails = "SELECT PRODUCTCODE, SALEQUANTITY, SELLINGPRICE FROM SALESDETAILS WHERE INVOICEID = @invoiceId";
            //var products = (List<ProductInvoiceDto>) await _connection.QueryAsync<ProductInvoiceDto>(queryForSalesDetails, new { invoiceId = data.INVOICEID });
            var products = await _appDbContext.SalesDetails.Where(s=> s.invoiceId == data.invoiceId).ToListAsync();

            var productDto = products.Adapt<List<ProductInvoiceDto>>();

            //var queryState = "SELECT * FROM STATES WHERE ID = @stateId";
            //var state = await _connection.QueryFirstOrDefaultAsync(queryState, new {stateId = data.DELIVERYSTATE});
            var state = await _appDbContext.States.FirstOrDefaultAsync(s=> s.id == data.deliveryState);

            //var queryCountry = "SELECT * FROM COUNTRIES WHERE ID = @countryId";
            //var country = await _connection.QueryFirstOrDefaultAsync(queryCountry, new { countryId = data.deliveryCountry });
            var country = await _appDbContext.Countries.FirstOrDefaultAsync(c => c.id == data.deliveryCountry);


            var response = new ResponseInvoiceDto
            {
                invoiceId = data.invoiceId,
                invoiceDate = data.invoiceDate,
                subTotal = data.subTotal,
                address = data.deliveryAddress,
                zipcode = data.deliveryZipcode,
                state = state.name,
                country = country.name,
                productInvoices = productDto
            };

            return response;
        }


    }

    


}
