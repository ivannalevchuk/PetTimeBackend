using PetTimeBackend.Entities;

namespace PetTimeBackend.IRepositories
{
    public interface IReportRepository
    {
        Report GetReportById(long reportId);
        IEnumerable<Report> GetReportsByUserId(long userId);
        void AddReport(Report report);
        void UpdateReport(Report report);
        void DeleteReport(long reportId);

        Task<List<Report>> GetAdvertisementsExcludingUserAsync(long userId);
        void Save();
    }
}
