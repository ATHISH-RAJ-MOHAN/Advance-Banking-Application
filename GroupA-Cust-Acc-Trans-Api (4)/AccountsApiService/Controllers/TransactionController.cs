using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace AccountsApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository<Transaction, long> _repository;

        public TransactionController(ITransactionRepository<Transaction, long> repository)
        {
            _repository = repository;
        }

        // GET: api/Transactions/{AccountId}
        [HttpGet("{AccountId}")]
        public async Task<IActionResult> GetAccountTransactions(long AccountId)
        {
            try
            {
                var model = _repository.GetAccountTransactions(AccountId);
                return Ok(model);
            }
            catch (SqlException sqlex)
            {
                return BadRequest(sqlex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /* // GET: api/Transactions/Specific/{AccountId}
         [HttpGet("Specific/{TransactionID}")]
         public async Task<IActionResult> GetTransactions(long TransactionID)
         {
             var model = _repository.GetTransactions(TransactionID);
             return Ok(model);
         }
 */

        // POST: api/Transactions/Transfer
        [HttpPost("Transfer")]
        public async Task<IActionResult> TransferFunds([FromBody] TransferRequest request)
        {
            try
            {
                _repository.TransferFunds(request.SourceAccountId, request.DestinationAccountId, request.Amount);
                return Ok(new { message = "Transfer successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet(template: "test")]
        public IActionResult Test()
        {
            return Ok("Api Connected and Up!");
        }
    }
    public class TransferRequest
    {
        public long SourceAccountId { get; set; }
        public long DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
