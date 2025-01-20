using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interfaces
{
    public interface ICountryStateService
    {
        public Task<List<Country>> getAllCountries();
        public Task<List<State>> getAllStates();
        public Task<List<State>> getStateByCountryId(int countryId);

    }
}
