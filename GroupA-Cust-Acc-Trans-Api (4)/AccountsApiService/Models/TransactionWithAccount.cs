using AccountsApiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsApiService.Models
{
    public class TransactionWithAccount
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public long Source_acc { get; set; }
        public long Dest_acc { get; set; }
        public Accounts AccountInfo { get; set; } // Navigation property to include account details

        public TransactionWithAccount()
        {
            AccountInfo = new Accounts(); // Initialize the account info object
        }
    }
}
