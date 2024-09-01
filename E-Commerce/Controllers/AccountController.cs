using E_Commerce.Domain.DTOs;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {        
            _accountService = accountService;
        }
       

        
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result= await _accountService.RegisterAsync(model);

            if(!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);  
        }
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.LoginAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.GetCurrentUserAsync();

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<IActionResult> GetCurrentUserAddress()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var addressDto = await _accountService.GetCurrentUserAddressAsync();

            if (addressDto == null)
                return NotFound("Address not found");

            return Ok(addressDto);
        }

        [Authorize]
        [HttpPut("UpdateCurrentUserAddress")]
        public async Task<IActionResult> UpdateCurrentUserAddress([FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updatedAddress = await _accountService.UpdateCurrentUserAddressAsync(addressDto);
                return Ok(updatedAddress);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _accountService.AddRoleAsync(model);

            if (!String.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }

    }
}
