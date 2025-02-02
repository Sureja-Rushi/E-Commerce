using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAddress(Address address)
        {
            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }

        public async Task<Address> GetAddressByIdAsync(int addressId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == addressId);
        }

        public async Task<List<Address>> GetAddressesByUserId(int userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }
    }
}
