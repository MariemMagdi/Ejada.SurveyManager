using Ejada.SurveyManager.Indicators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;

namespace Ejada.SurveyManager.Controllers
{
    [Route("api/app/indicator")]
    public class IndicatorController : SurveyManagerController
    {
        private readonly IIndicatorAppService _indicatorAppService;

        public IndicatorController(IIndicatorAppService indicatorAppService)
        {
            _indicatorAppService = indicatorAppService;
        }

        [HttpGet]
        [Route("{id:guid}/export-pdf")]
        [Authorize]
        public async Task<IActionResult> ExportPdf(Guid id, [FromQuery] bool exportAllResponses = true)
        {
            try
            {
                var stream = await _indicatorAppService.ExportIndicatorReportPdfAsync(id, exportAllResponses);
                
                var indicator = await _indicatorAppService.GetAsync(id);
                var fileName = $"Indicator_{indicator.Name.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
                fileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^\w\-_\.]", "_");

                return File(stream, "application/pdf", fileName);
            }
            catch (BusinessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

