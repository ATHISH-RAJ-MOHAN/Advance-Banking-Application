using FinalProject.DataAccess;
using FinalProject.Model;
using FinalProject.Process;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class EnquiryLoginController : Controller
    {
        private readonly EnquiryLoginProcess _process;

        public EnquiryLoginController(EnquiryLoginProcess process)
        {

            _process = process;
        }



        [HttpPost(template: "AddEnquirer")]
        public IActionResult CreateEnquirer(EnquiryLogin model)
        {
            try
            {
                string result = "";

                result = _process.CreateEnquirer(model.Email, model.Password);

                if (result.Length > 0) { return BadRequest(); }
                else return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(template: "GetEnquirer")]
        public IActionResult GetEnquirer(EnquiryLogin model)
        {
            try
            {
                CreateEnquiry model2 = null;
                model2 = _process.GetEnquirer(model.Email, model.Password);


                if (model2 is null)
                {
                    return BadRequest("User not found");
                }
                else
                {

                    return Ok(model2);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(template: "GetDocuments")]
        public IActionResult GetDocuments(Emailing model1)
        {
            try
            {
                Document model = null;
                model = _process.GetDocuments(model1.Email);



                return Ok(model);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        

        [HttpPost(template: "CreateDocuments")]
        public IActionResult CreateDocuments(Document model)
        {

            try
            {
                byte[] photoBytes = Utility.ConvertToBytes(model.Photo);
                byte[] aadharBytes = Utility.ConvertToBytes(model.Aadhar);
                byte[] pancardBytes = Utility.ConvertToBytes(model.PanCard);


                _process.CreateDocuments(
                 model.Email,
                 photoBytes,
                 aadharBytes,
                 pancardBytes
                 );
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost(template: "CreateEnquiry")]
        public IActionResult CreateEnquiry(CreateEnquiry model)
        {
            try
            {

                _process.CreateEnquiry(
                model.FirstName,
                model.LastName,
                model.Address1,
                model.Address2,
                model.Address3,
                model.PhoneNumber,
                model.Email,
                model.DOB,
                model.City,
                model.Country,
                model.Status,
                model.Pincode,
                model.WantsCheque,
                model.Feedback,
                model.IsActive,
                model.AccountType,
                model.Balance

                 );
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost(template: "SaveEnquiry")]
        public IActionResult SaveEnquiry(CreateEnquiry model)
        {
            try
            {
                _process.SaveEnquiry(
            model.FirstName,
            model.LastName,
            model.Address1,
            model.Address2,
            model.Address3,
            model.PhoneNumber,
            model.Email,
            model.DOB,
            model.City,
            model.Country,
            model.Status,
            model.Pincode,
            model.WantsCheque,
            model.Feedback,
            model.IsActive,
            model.AccountType,
            model.Balance
            );
                return Ok();



            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(template: "test")]
        public IActionResult Test()
        {
            return Ok("Api Connected and Up!");
        }
    }
}
