using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Entities;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace PetTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public IActionResult CreateReport([FromForm] ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //using (var stream = System.IO.File.OpenRead(model.Image))
                //{
                //    var extension = Path.GetExtension(model.Image);
                //    var contentType = GetContentType(extension);
                //    var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                //    {

                //        Headers = new HeaderDictionary(),
                //        ContentType = contentType
                //    };
                
                    _reportService.CreateReport(model, model.Image);

                    return Ok(new { Message = "Report added successfully" });
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReport(long id, [FromBody] ReportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                //model.Id = id;
                _reportService.UpdateReport(model, id);
                return Ok(new { Message = "Report updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReport(long id)
        {
            try
            {
                _reportService.DeleteReport(id);
                return Ok(new { Message = "Report deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetReportsByUserId(long userId)
        {
            try
            {
                var reports = _reportService.GetReportsByUserId(userId);
                if (!reports.Any())
                {
                    return NotFound("No reports found for the specified user");
                }

                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetReportById(long id)
        {
            try
            {
                var report = _reportService.GetReportById(id);
                if (report == null)
                {
                    return NotFound("Report not found");
                }

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("exclude-user/{userId}")]
        public async Task<IActionResult> GetReportsExcludingUser(long userId)
        {
            try
            {
                var ads = await _reportService.GetAdvertisementsExcludingUserAsync(userId);
                if (ads == null || ads.Count == 0)
                {
                    return NotFound("No reports found.");
                }

                return Ok(ads);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
       
    }
}
