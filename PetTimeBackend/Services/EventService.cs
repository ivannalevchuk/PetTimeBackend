using Hangfire;
using PetTimeBackend.Entities;
using PetTimeBackend.IRepositories;
using PetTimeBackend.IServices;
using PetTimeBackend.ViewModels;

namespace PetTimeBackend.Services
{
    public class EventService: IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly FCMService _fcmService;
        private readonly FirestoreService _firestoreService;
        private readonly IPetRepository _petRepository;
        public EventService(IEventRepository eventRepository, FCMService fcmService, FirestoreService firestoreService, IPetRepository petRepository)
        {
            _eventRepository = eventRepository;
            _fcmService = fcmService;
            _firestoreService = firestoreService;
            _petRepository = petRepository;
        }

        public Event GetEventById(long eventId)
        {
            return _eventRepository.GetEventById(eventId);
        }

        public IEnumerable<Event> GetEventsByPetId(long petId)
        {
            return _eventRepository.GetEventsByPetId(petId);
        }

        public IEnumerable<Event>GetEventsByDate (string date)
        {
            return _eventRepository.GetEventsByDate(date);
        }

        public async Task RemoveEvent (long eventId)
        {
            await _eventRepository.RemoveEvent(eventId);
        }
        public void CreateEvent(EventViewModel model)
        {
            var pet = _eventRepository.GetPetById(model.PetId);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            var newEvent = new Event
            {
                PetId = model.PetId,
                EventDate = model.EventDate,
                EventType = model.EventType,
                Description = model.Description
            };

            _eventRepository.AddEvent(newEvent);
            _eventRepository.Save();
            var eventTime = model.EventDate;

            // Розрахуйте час нагадування
            var reminderTime = eventTime.AddHours(-1);
            //var reminderTime = TimeZoneInfo.ConvertTimeToUtc(eventTime.AddHours(-1), TimeZoneInfo.Local);
            // Заплануйте задачу у Hangfire
            BackgroundJob.Schedule(() => SendEventReminder(model.PetId, model.Description), reminderTime);
        }
        public async Task SendEventReminder(long petId, string eventDescription)
        {

            var userId = GetOwnerIdByPetId(petId);
            Console.WriteLine("User id: " + userId);
            // Отримайте токен користувача, який асоційований з petId (це залежить від вашої логіки)
            var userToken = _firestoreService.GetFCMTokenAsync(userId.ToString()); // Це потрібно отримати з БД або іншого джерела
            
           await _fcmService.SendNotificationAsync(userToken.Result, "Event Reminder", $"Your event {eventDescription} will start in one hour.");
        }
        public long GetOwnerIdByPetId(long petId)
        {
            var pet = _petRepository.GetPetById(petId);
            if (pet == null)
            {
                throw new Exception("Pet not found");
            }

            // Отримайте ідентифікатор власника тварини за допомогою petId (це приклад)
            return pet.UserId;
        }
    }
}
