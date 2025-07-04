namespace GroupA_Manager_Api.Models
{
    public class EnquiryModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string PinCode { get; set; }
        public bool WantsCheque { get; set; }
        public string Feedback { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
        public string Photo { get; set; }
        public string Aadhaar { get; set; }
        public string panCard { get; set; }
    }
}
