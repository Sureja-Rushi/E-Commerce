using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class CreateOrderRequestDTO : IValidatableObject
    {
        public int? ExistingAddressId { get; set; }

        public AddressDTO? NewAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ExistingAddressId.HasValue && NewAddress == null)
            {
                yield return new ValidationResult(
                    "Either ExistingAddressId or NewAddress must be provided.",
                    new[] { nameof(ExistingAddressId), nameof(NewAddress) });
            }

            if (ExistingAddressId.HasValue && NewAddress != null)
            {
                yield return new ValidationResult(
                    "Provide only one: ExistingAddressId or NewAddress.",
                    new[] { nameof(ExistingAddressId), nameof(NewAddress) });
            }
        }
    }

    public class AddressDTO
    {
        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string ContactNumber { get; set; }
    }
}
