using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        [HttpGet]
        #region Get All Countries
        public async Task<IActionResult> GetCountries(int page)
        {
            try
            {
                var countries = await _regionService.GetCountriesAsync(page);
                if (countries.Values.Count == 0)
                    return BadRequest("Country Not Found...");

                return Ok(countries);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All States By Country
        public async Task<IActionResult> GetStatesByCountry(int countryId, int page)
        {
            try
            {
                var states = await _regionService.GetstateByCountry(countryId, page);
                if (states.Values.Count == 0)
                    return BadRequest("State Not Found...");

                return Ok(states);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpGet]
        #region Get All Cities By State
        public async Task<IActionResult> GetcitiesByState(int stateId, int page)
        {
            try
            {
                var cities = await _regionService.GetCityByState(stateId, page);
                if (cities.Values.Count == 0)
                    return BadRequest("City Not Found...");

                return Ok(cities);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
