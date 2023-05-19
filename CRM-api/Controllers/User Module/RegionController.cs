using CRM_api.DataAccess.Helper;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        public async Task<IActionResult> GetCountries([FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var countries = await _regionService.GetCountriesAsync(data, sortingParams);
                if (countries.Values.Count == 0)
                    return BadRequest(new { Message = "Country not found."});

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
        public async Task<IActionResult> GetStatesByCountry(int countryId, [FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var states = await _regionService.GetstateByCountry(countryId, data, sortingParams);
                if (states.Values.Count == 0)
                    return BadRequest(new { Message = "State not found."});

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
        public async Task<IActionResult> GetcitiesByState(int stateId, [FromHeader] string? searchingParams, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var data = new Dictionary<string, object>();
                if (searchingParams != null)
                {
                    data = JsonSerializer.Deserialize<Dictionary<string, object>>(searchingParams,
                        new JsonSerializerOptions
                        {
                            Converters =
                            {
                            new ObjectDeserializer()
                            }
                        });
                }
                var cities = await _regionService.GetCityByState(stateId, data, sortingParams);
                if (cities.Values.Count == 0)
                    return BadRequest(new { Message = "City not found."});

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
        public async Task<IActionResult> DeactivateCountry(int countryId)
        {
            var country = await _regionService.DeactivateCountryAsync(countryId);
            return country != 0 ? Ok(new { Message = "Country deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate country."});
        }
        #endregion

        [HttpDelete]
        #region Deactivate State
        public async Task<IActionResult> DeactivateState(int stateId)
        {
            var state = await _regionService.DeactivateStateAsync(stateId);
            return state !=0 ? Ok(new { Message = "State deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate state."});
        }
        #endregion

        [HttpDelete]
        #region Deactivate City
        public async Task<IActionResult> DeactivateCity(int cityId)
        {
            var city = await _regionService.DeactivateCityAsync(cityId);
            return city != 0 ? Ok(new { Message = "City deactivated successfully."}) : BadRequest(new { Message = "Unable to deactivate city."});
        }
        #endregion
    }
}
