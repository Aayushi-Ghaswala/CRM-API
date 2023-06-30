using CRM_api.DataAccess.Helper;
using CRM_api.Services.Dtos.AddDataDto.User_Module;
using CRM_api.Services.IServices.User_Module;
using Microsoft.AspNetCore.Mvc;

namespace CRM_api.Controllers.User_Module
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;

        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        #region Get All Countries
        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries([FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var countries = await _regionService.GetCountriesAsync(search, sortingParams);

                return Ok(countries);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All States By Country
        [HttpGet("GetStatesByCountry")]
        public async Task<IActionResult> GetStatesByCountry(int countryId, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var states = await _regionService.GetstateByCountryAsync(countryId, search, sortingParams);

                return Ok(states);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get All Cities By State
        [HttpGet("GetcitiesByState")]
        public async Task<IActionResult> GetcitiesByState(int stateId, [FromQuery] string? search, [FromQuery] SortingParams? sortingParams)
        {
            try
            {
                var cities = await _regionService.GetCityByStateAsync(stateId, search, sortingParams);

                return Ok(cities);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add Country
        [HttpPost("AddCountry")]
        public async Task<IActionResult> AddCountry(AddCountryDto countryDto)
        {
            try
            {
                var role = await _regionService.AddCountryAsync(countryDto);
                return role != 0 ? Ok(new { Message = "Country added successfully." }) : BadRequest(new { Message = "Country already exists." });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add State
        [HttpPost("AddState")]
        public async Task<IActionResult> AddState(AddStateDto stateDto)
        {
            try
            {
                var role = await _regionService.AddStateAsync(stateDto);
                return role != 0 ? Ok(new { Message = "State added successfully." }) : BadRequest(new { Message = "State already exists." });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Add City
        [HttpPost("AddCity")]
        public async Task<IActionResult> AddCity(AddCityDto cityDto)
        {
            try
            {
                var role = await _regionService.AddCityAsync(cityDto);
                return role != 0 ? Ok(new { Message = "City added successfully." }) : BadRequest(new { Message = "City already exists." });

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update Country
        [HttpPut("UpdateCountry")]
        public async Task<IActionResult> UpdateCountry(UpdateCountryDto countryDto)
        {
            try
            {
                var role = await _regionService.UpdateCountryAsync(countryDto);
                return role != 0 ? Ok(new { Message = "Country updated successfully." }) : BadRequest(new { Message = "Unable to update country." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update State
        [HttpPut("UpdateState")]
        public async Task<IActionResult> UpdateState(UpdateStateDto stateDto)
        {
            try
            {
                var role = await _regionService.UpdateStateAsync(stateDto);
                return role != 0 ? Ok(new { Message = "State updated successfully." }) : BadRequest(new { Message = "State to update state." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Update City
        [HttpPut("UpdateCity")]
        public async Task<IActionResult> UpdateCity(UpdateCityDto cityDto)
        {
            try
            {
                var role = await _regionService.UpdateCityAsync(cityDto);
                return role != 0 ? Ok(new { Message = "City updated successfully." }) : BadRequest(new { Message = "Unable to update city." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate Country
        [HttpDelete("DeactivateCountry")]
        public async Task<IActionResult> DeactivateCountry(int countryId)
        {
            try
            {
                var country = await _regionService.DeactivateCountryAsync(countryId);
                return country != 0 ? Ok(new { Message = "Country deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate country." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate State
        [HttpDelete("DeactivateState")]
        public async Task<IActionResult> DeactivateState(int stateId)
        {
            try
            {
                var state = await _regionService.DeactivateStateAsync(stateId);
                return state != 0 ? Ok(new { Message = "State deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate state." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Deactivate City
        [HttpDelete("DeactivateCity")]
        public async Task<IActionResult> DeactivateCity(int cityId)
        {
            try
            {
                var city = await _regionService.DeactivateCityAsync(cityId);
                return city != 0 ? Ok(new { Message = "City deactivated successfully." }) : BadRequest(new { Message = "Unable to deactivate city." });
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
