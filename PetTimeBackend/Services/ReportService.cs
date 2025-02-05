using Azure.Storage.Blobs;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Mvc;
using PetTimeBackend.Entities;
using PetTimeBackend.IRepositories;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace PetTimeBackend.Services
{
    public class ReportService: IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly FirestoreService _firestoreService;
        private readonly FCMService _fcmService;
        public ReportService(IReportRepository reportRepository, BlobServiceClient blobServiceClient, FirestoreService firestoreService, FCMService fcmService)
        {
            _reportRepository = reportRepository;
            _blobServiceClient = blobServiceClient;
            _firestoreService = firestoreService;
            _fcmService = fcmService;
        }

        public Report GetReportById(long reportId)
        {
            return _reportRepository.GetReportById(reportId);
        }

        public IEnumerable<Report> GetReportsByUserId(long userId)
        {
            return _reportRepository.GetReportsByUserId(userId);
        }

        public async Task CreateReport(ReportViewModel model, IFormFile image)
        {
            string imageUrl = null;

            if (image != null)
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient("images");
                var blobClient = containerClient.GetBlobClient(Guid.NewGuid().ToString() + Path.GetExtension(image.FileName));

                using (var stream = image.OpenReadStream())
                {
                     blobClient.Upload(stream);
                }

                imageUrl = blobClient.Uri.ToString();
            }
            var report = new Report
            {
                UserId = model.UserId,
                PetName = model.PetName,
                Breed = model.Breed,
                Sex = model.Sex,
                PhoneNumber = model.PhoneNumber,
                Nickname = model.Nickname,
                Longitude = model.Longitude,
                Latitude = model.Latitude,
                ImageUrl = imageUrl
            };

            _reportRepository.AddReport(report);
            _reportRepository.Save();
            Console.WriteLine("UserID: "  + model.UserId);
            var otherUserLocations = await _firestoreService.GetAllUserLocationsAsync(model.UserId);
            Console.WriteLine(otherUserLocations.Count);
            foreach (var userLocation in otherUserLocations)
            {
             
                double distance = DistanceCalculator.CalculateDistance(model.Latitude, model.Longitude, userLocation.Latitude, userLocation.Longitude);
                if (distance <= 0.5) // 500 meters
                {
                    // Send FCM notification
                    var notificationTitle = "Missing pet nearby!";
                    var notificationBody = $"{model.PetName} is lost! Have you seen it?";

                    await _fcmService.SendNotificationAsync(userLocation.FcmToken, notificationTitle, notificationBody);
                }
            }
           
        }

        public void UpdateReport(ReportViewModel model, long userId)
        {
            var report = _reportRepository.GetReportById(userId);
            if (report == null)
            {
                throw new Exception("Report not found");
            }

            report.PetName = model.PetName;
            report.Breed = model.Breed;
            report.Sex = model.Sex;
            report.PhoneNumber = model.PhoneNumber;
            report.Nickname = model.Nickname;
            report.Longitude = model.Longitude;
            report.Latitude = model.Latitude;

            _reportRepository.UpdateReport(report);
            _reportRepository.Save();


        }

        public void DeleteReport(long reportId)
        {
            _reportRepository.DeleteReport(reportId);
            _reportRepository.Save();
        }

        public async Task<List<Report>> GetAdvertisementsExcludingUserAsync(long userId)
        {
            return await _reportRepository.GetAdvertisementsExcludingUserAsync(userId);
        }
    }
}
