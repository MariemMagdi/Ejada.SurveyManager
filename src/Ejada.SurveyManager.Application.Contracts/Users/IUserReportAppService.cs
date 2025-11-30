using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Ejada.SurveyManager.Users
{
    public interface IUserReportAppService : IApplicationService
    {
        /// <summary>
        /// Exports user report as PDF
        /// </summary>
        Task<Stream> ExportUserReportPdfAsync(Guid userId);
    }
}

