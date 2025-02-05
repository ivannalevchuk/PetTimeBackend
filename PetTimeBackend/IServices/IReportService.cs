using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Entities;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.IServices
{
    public interface IReportService
    {
        Report GetReportById(long reportId);
        IEnumerable<Report> GetReportsByUserId(long userId);
        Task CreateReport(ReportViewModel model, IFormFile image);
        void UpdateReport(ReportViewModel model, long UserId);
        void DeleteReport(long reportId);
        Task<List<Report>> GetAdvertisementsExcludingUserAsync(long userId);
    }
}
