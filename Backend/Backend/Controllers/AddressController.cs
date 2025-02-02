using Backend.Models;
using Backend.Helpers;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend.DTOs;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/address")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAddressesByUserId()
        {
            var token = Request.Cookies["AuthToken"];
            var user = JwtTokenHelper.GetUserFromToken(token);
            var addresses = await _addressService.GetAddressesByUserId(user.Id);
            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddAddressDTO addressDto)
        {
            var token = Request.Cookies["AuthToken"];
            var user = JwtTokenHelper.GetUserFromToken(token);
            await _addressService.AddAddress(addressDto, user);
            //return CreatedAtAction(nameof(GetAddressesByUserId), new { user.Id }, address);
            return Ok(new { message = "Address added successfully!" });
        }
    }
}