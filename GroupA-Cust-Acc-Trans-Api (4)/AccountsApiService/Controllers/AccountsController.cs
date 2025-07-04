using AccountsApiService.Infrastructure;
using AccountsApiService.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace AccountsApiService.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    [EnableCors]
    public class AccountsController : ControllerBase
    {
        private readonly IRepository<Accounts, long> _repository;

        public AccountsController(IRepository<Accounts, long> repository)
        {
            _repository = repository;
        }

        // GET: api/Accounts
        [HttpGet(template: "{CustId}")]
        public async Task<IActionResult> GetAllAccountsByCustomerId(Int32 CustId)
        {
            var model = _repository.GetAllAccountsByCustomerID(CustId);
            return Ok(model);
        }

        // GET: api/Accontss/5
        [HttpGet(template: "AccId:{AccId}")]
        public async Task<ActionResult<Accounts>> GetAccountByAccountId(long AccId)
        {
            var model = _repository.GetAccountByAccountID(AccId);
            if (model is not null)
            {
                return model;
            }
            else
                return NotFound();
        }
        [HttpPut("ApplyCheque/{AccId}")]
        public IActionResult ApplyCheque(long AccId)
        {
            try
            {
                _repository.ApplyCheque(AccId);
                return Ok(); // Return 200 OK if operation succeeds
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error if an exception occurs
            }
        }

        [HttpPost("Create")]
        public ActionResult<Accounts> CreateAccount(Accounts model)
        {
            if (model == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _repository.CreateAccount(model);
            return Ok(model);
        }
        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<Accounts>> DeleteAccount(long AccId)
        {
            _repository.DeleteAccountByAccountId(AccId);
            return Ok();
        }


    }
}