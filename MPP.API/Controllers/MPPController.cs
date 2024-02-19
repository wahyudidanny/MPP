
using Microsoft.AspNetCore.Mvc;
using MPP.API.Service;

namespace MPP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MPPController : ControllerBase
    {
        private readonly MPPService _MPPService;

        public MPPController(MPPService MPPService)
        {
            _MPPService = MPPService;
        }

        [HttpGet("GenerateApprovalMPP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GenerateApprovalMPP(string company, string location,string tahun, string bulan)
        {

            bool? generatePdf = await _MPPService.generateApprovalMPP(company,  location, tahun,  bulan);

            if (generatePdf != null)
                return Ok("Successfull Generate PDF");
            else
                return NotFound("Generate PDF Failed!! Please Check Your Log");
                
        }

    }

}