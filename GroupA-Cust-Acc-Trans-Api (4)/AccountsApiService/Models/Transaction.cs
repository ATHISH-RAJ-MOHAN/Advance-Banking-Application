using System.ComponentModel.DataAnnotations;

namespace AccountsApiService.Models
{
    public class Transaction
    {
        [Required(ErrorMessage = "Transaction ID is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "TransactionID must contain only numeric digits.")]
        public int TransactionID { get; set; }

        public decimal Amount { get; set; }

        public DateTime Time { get; set; }

        [Required(ErrorMessage = "Account ID is Required")]
        public long Source_acc { get; set; }

        [Required(ErrorMessage = "Account ID is Required")]
        public long Dest_acc { get; set; }

        

    }
}
