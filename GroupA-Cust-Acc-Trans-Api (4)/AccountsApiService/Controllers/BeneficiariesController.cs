using AccountsApiService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccountsApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IBeneficiariesRepository _beneficiariesRepository;

        public BeneficiariesController(IBeneficiariesRepository beneficiariesRepository)
        {
            _beneficiariesRepository = beneficiariesRepository ?? throw new ArgumentNullException(nameof(beneficiariesRepository));
        }

        [HttpPost("AddBeneficiary")]
        public async Task<IActionResult> AddBeneficiary(long sourceAcc, long destAcc)
        {
            try
            {
                await _beneficiariesRepository.AddBeneficiaryAsync(sourceAcc, destAcc);
                return Ok("Beneficiary added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetBeneficiaries/{sourceAcc}")]
        public async Task<IActionResult> GetBeneficiaries(long sourceAcc)
        {
            try
            {
                var beneficiaries = await _beneficiariesRepository.GetBeneficiariesBySourceAccAsync(sourceAcc);
                return Ok(beneficiaries);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("DeactivateBeneficiary")]
        public async Task<IActionResult> DeactivateBeneficiary(long sourceAcc, long destAcc)
        {
            try
            {
                await _beneficiariesRepository.DeactivateBeneficiaryAsync(sourceAcc, destAcc);
                return Ok("Beneficiary deactivated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
