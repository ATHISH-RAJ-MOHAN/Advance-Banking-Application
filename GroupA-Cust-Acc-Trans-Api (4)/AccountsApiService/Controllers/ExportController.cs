using AccountsApiService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AccountsApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly Stx _stx;

        public ExportController(Stx stx)
        {
            _stx = stx;
        }

        [HttpGet("GenerateAndSave/{id}")]
        public IActionResult GenerateAndSave(long id)
        {
            string storedProcedureName = "sp_GetAccountTransactions";
            string fileName = $"Report_{id}.xlsx";

            DataTable dataTable = _stx.ExecuteStoredProcedure(storedProcedureName, id);
            byte[] excelFile = _stx.CreateExcelFile(dataTable);

            _stx.SaveFileToDatabase(fileName, excelFile, id);

            return Ok("File generated and saved successfully.");
        }

        [HttpGet("DownloadFile/{id}")]
        public IActionResult DownloadFile(long id)
        {
            var (fileData, fileName) = _stx.GetLatestFileFromDatabase(id);

            if (fileData == null)
                return NotFound();

            return File(fileData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
