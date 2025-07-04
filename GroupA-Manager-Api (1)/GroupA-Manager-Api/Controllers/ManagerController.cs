using GroupA_Manager_Api.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GroupA_Manager_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ManagerController : ControllerBase
    {
        private readonly IMailService _mailService;
        private readonly IManagerRepository<MailData> _mailRepository;

        public ManagerController(IMailService mailService, IManagerRepository<MailData> mailRepository)
        {

            _mailService = mailService;
            _mailRepository = mailRepository;
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Api Connected and Up!");
        }

        [HttpGet("TestMail")]
        public async Task<IActionResult> TestMail(int id)
        {
            try
            {
                await _mailService.TestEmail();
                return Ok("Mail has been Sent Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            try
            {
                await _mailService.SendEmailAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Reject")]
        public async Task<IActionResult> Reject(FeedbackModel model)
        {
            try
            {
                await _mailService.SendRejectedMaiAsync(model.Id, model.Feedback);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Managerid/{id}")]
        public ActionResult<int> ManagerId(int id)
        {
            try
            {
                int? ManagerId = _mailRepository.GetManagerIdFromUserId(id);
                return Ok(ManagerId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PendingList/{id}")]
        public ActionResult<RequestModel> PendingList(int id)
        {
            try
            {
                var PendingEnquiries = _mailRepository.PendingEnquiries(id);
                return Ok(PendingEnquiries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("RejectedList/{id}")]
        public ActionResult<RequestModel> RejectedList(int id)
        {
            try
            {
                var PendingEnquiries = _mailRepository.RejectedEnquiries(id);
                return Ok(PendingEnquiries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ApprovedList/{id}")]
        public ActionResult<RequestModel> ApprovedList(int id)
        {
            try
            {
                var PendingEnquiries = _mailRepository.ApprovedEnquiries(id);
                return Ok(PendingEnquiries);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EnquiryDetails/{id}")]
        public ActionResult<EnquiryModel> EnquiryDetails(int id)
        {
            try
            {
                var model = _mailRepository.DisplayEnquiry(id);
                if (model is null) return NotFound(new { message = "Enquiry Not Found"});
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

    }
}
