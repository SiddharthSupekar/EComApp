using App.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EComApplicationBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryStateController : ControllerBase
    {
        private readonly ICountryStateService _countryStateService;

        public CountryStateController(ICountryStateService countryStateService)
        {
            _countryStateService  = countryStateService;
        }

        [HttpGet("countries")]
        public async Task<IActionResult> getCountry()
        {
            var countries = await _countryStateService.getAllCountries();
            return Ok(countries);
        }

        [HttpGet("states")]
        public async Task<IActionResult> getState()
        {
            var states = await _countryStateService.getAllStates();
            return Ok(states);  
        }

        [HttpGet("country/states")]
        public async Task<IActionResult> getStateByCountryId(int countryId)
        {
            if(countryId != null)
            {
                var states = await _countryStateService.getStateByCountryId(countryId);

                if(states != null)
                {
                    return Ok(states);
                }
                return NotFound($"No state for the country");
            }  
            return BadRequest();
        }
    }
}
