using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Microsoft.EntityFrameworkCore;
using PetTimeBackend.Contexts;
using PetTimeBackend.Entities;

namespace PetTimeBackend.IRepositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly PetTimeContext _context;
        private readonly GoogleMapsService _mapsService;
      
        public ReportRepository(PetTimeContext context)
        {
            _context = context;
        }

        public Report GetReportById(long reportId)
        {
            return _context.Reports.Find(reportId);
        }
        
        public IEnumerable<Report> GetReportsByUserId(long userId)
        {
            return _context.Reports.Where(r => r.UserId == userId).ToList();
        }

        public void AddReport(Report report)
        {
            _context.Reports.Add(report);
        }

        public void UpdateReport(Report report)
        {
            _context.Reports.Update(report);
        }

        public void DeleteReport(long reportId)
        {
            var report = _context.Reports.Find(reportId);
            if (report != null)
            {
                _context.Reports.Remove(report);
            }
        }
        public async Task<List<Report>> GetAdvertisementsExcludingUserAsync(long userId)
        {
            return await _context.Reports
                                   .Where(ad => ad.UserId != userId)
                                   .ToListAsync();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
