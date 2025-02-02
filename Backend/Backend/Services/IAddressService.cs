using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IAddressService
    {
        Task<List<Address>> GetAddressesByUserId(int userId);
        //Task AddAddress(Address address, int userId);
        Task AddAddress(AddAddressDTO addressDto, User user);
    }
}
