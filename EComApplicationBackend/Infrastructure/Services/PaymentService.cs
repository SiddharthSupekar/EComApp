using App.Core.Dto;
using App.Core.Interfaces;
using Dapper;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly SqlConnection _connection;

        public PaymentService(AppDbContext appDbContext, IConfiguration configuration)
        {
           _appDbContext = appDbContext;
           _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));

        }


        public async Task<bool> verifyCard(CardDto cardDto)
        {
            var query = "SELECT * FROM CARDDETAILS WHERE ID = 1";
            var data = (List<CardDetails>) await _connection.QueryAsync<CardDetails>(query);
            

            if (data[0].cardNumber == cardDto.cardNumber && data[0].expiryDate == cardDto.expiryDate && data[0].cvv == cardDto.cvv)
            {
                return true;
            }
            return false;
        }


    }
}
