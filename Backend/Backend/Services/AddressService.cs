using Backend.Models;
using Backend.Repositories;
using Backend.DTOs;

namespace Backend.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;

        public AddressService(IAddressRepository addressRepository, IUserRepository userRepository)
        {
            _addressRepository = addressRepository;
            _userRepository = userRepository;
        }

        public async Task AddAddress(AddAddressDTO addressDto, User user) //addressDto, user
        {
            //address.UserId = userId; // Set the UserId from the logged-in user
            //await _addressRepository.AddAddress(address);

            Address newAddress = new Address
            {
                UserId = user.Id,
                Street = addressDto.Street,
                City = addressDto.City,
                State = addressDto.State,
                ZipCode = addressDto.ZipCode,
                FirstName = addressDto.FirstName, //FirstName = firstName
                LastName = addressDto.LastName, //LastName = lastName
                ContactNumber = addressDto.ContactNumber, //ContactNumber = contactNumber
                //User = user
            };

            await _addressRepository.AddAddress(newAddress);
        }

        public async Task<List<Address>> GetAddressesByUserId(int userId)
        {
            return await _addressRepository.GetAddressesByUserId(userId);
        }
    }
}
