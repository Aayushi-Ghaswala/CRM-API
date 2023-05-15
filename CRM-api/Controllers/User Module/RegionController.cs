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

        [HttpDelete]
        #region Deactivate Country
        public async Task<IActionResult> DeactivateCountry(int CountryId)
        {
            var country = await _regionService.DeactivateCountryAsync(CountryId);
            return country != 0 ? Ok("Country deactivated successfully.") : BadRequest("Unable to deactivate country.");
        }
        #endregion

        [HttpDelete]
        #region Deactivate State
        public async Task<IActionResult> DeactivateState(int StateId)
        {
            var state = await _regionService.DeactivateStateAsync(StateId);
            return state !=0 ? Ok("State deactivated successfully.") : BadRequest("Unable to deactivate state.");
        }
        #endregion

        [HttpDelete]
        #region Deactivate City
        public async Task<IActionResult> DeactivateCity(int CityId)
        {
            var city = await _regionService.DeactivateCityAsync(CityId);
            return city != 0 ? Ok("City deactivated successfully.") : BadRequest("Unable to deactivate city.");
        }
        #endregion
    }
}
