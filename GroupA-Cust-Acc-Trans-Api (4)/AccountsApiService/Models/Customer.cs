using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AccountsApiService.Models
{
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address line 1 is required.")]
        [StringLength(100, ErrorMessage = "Address line 1 cannot be longer than 100 characters.")]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        [Required(ErrorMessage = "Pincode is required.")]
        // [RegularExpression(@"^\d{4,6}$", ErrorMessage = "Pincode must be between 4 and 6 digits.")]
        public int Pincode { get; set; }

        [Required(ErrorMessage = "PhoneNumber is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot be longer than 15 characters.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Eail is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "DOB is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot be longer than 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required.")]
        [StringLength(50, ErrorMessage = "Country cannot be longer than 50 characters.")]
        public string Country { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Enquiry_ID { get; set; }
    }
}
