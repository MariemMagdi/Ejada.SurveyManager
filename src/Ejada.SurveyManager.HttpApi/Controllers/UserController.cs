using Ejada.SurveyManager.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Identity;

namespace Ejada.SurveyManager.Controllers
{
    [Route("api/app/user")]
    public class UserController : SurveyManagerController
    {
        private readonly IUserReportAppService _userReportAppService;
        private readonly IIdentityUserAppService _identityUserAppService;

        public UserController(IUserReportAppService userReportAppService, IIdentityUserAppService identityUserAppService)
        {
            _userReportAppService = userReportAppService;
            _identityUserAppService = identityUserAppService;
        }

        [HttpGet]
        [Route("{id:guid}/export-report-pdf")]
        [Authorize]
        public async Task<IActionResult> ExportReportPdf(Guid id)
        {
            try
            {
                var stream = await _userReportAppService.ExportUserReportPdfAsync(id);
                
                // Get user email for filename
                var user = await _identityUserAppService.GetAsync(id);
                var userEmail = user.Email ?? user.UserName;
                var fileName = $"UserReport_{userEmail.Replace("@", "_").Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
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

