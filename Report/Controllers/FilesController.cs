using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Report.Services;
using System.Threading.Tasks;

namespace Report.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public FilesController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpGet]
        [Route("v1/get")]
        public async Task<IActionResult> Get()
        {
            var file = await _pdfService.CreateDemoReport();
            return File(file, "application/pdf", "file.pdf");
        }            
    }
}
