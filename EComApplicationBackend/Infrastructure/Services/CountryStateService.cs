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
    public class CountryStateService : ICountryStateService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly SqlConnection _connection;

        public CountryStateService(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _connection = new SqlConnection(configuration.GetConnectionString("EComApplicationConnectionString"));
        }

        public async Task<List<Country>> getAllCountries()
        {
            var query = "SELECT * FROM COUNTRIES";

            var countries =(List<Country>) await _connection.QueryAsync<Country>(query);

            return countries;
        }

        public async Task<List<State>> getAllStates()
        {
            var query = "SELECT * FROM STATES";

            var states = (List<State>) await _connection.QueryAsync<State>(query);

            return states;
        }

        public async Task<List<State>> getStateByCountryId(int countryId)
        {
            var query = "SELECT * FROM states WHERE countryId = @countryId";

            var states = (List<State>) await _connection.QueryAsync<State>(query, new {countryId = countryId});

            return states;
        }
    }
}
