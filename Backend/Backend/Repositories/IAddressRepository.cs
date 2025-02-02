using Backend.Models;

namespace Backend.Repositories
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetAddressesByUserId(int userId);
        Task AddAddress(Address address);
        Task<Address> GetAddressByIdAsync(int addressId);
    }
}
