using Google.Cloud.Firestore;


namespace PetTimeBackend.Services
{
    public class FirestoreService
    {
        private readonly FirestoreDb _firestoreDb;

        public FirestoreService(string projectId)
        {
            _firestoreDb = FirestoreDb.Create(projectId);
        }

        public async Task<string> GetFCMTokenAsync(string userId)
        {
            try
            {
                // Отримуємо документ користувача з Firestore
                DocumentReference docRef = _firestoreDb.Collection("users").Document(userId);
                DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

                // Перевіряємо, чи існує документ та чи містить він токен FCM
                if (snapshot.Exists && snapshot.TryGetValue<string>("fcmToken", out var fcmToken) && fcmToken != null)
                {
                    return  fcmToken.ToString();
                }
                else
                {
                    throw new Exception("FCM token not found for the user");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving FCM token: {ex.Message}");
                return null;
            }
        }
        public async Task SaveUserLocationAsync(string userId, double latitude, double longitude)
        {
            DocumentReference docRef = _firestoreDb.Collection("users").Document(userId);
            Dictionary<string, object> location = new Dictionary<string, object>
        {
            { "latitude", latitude },
            { "longitude", longitude },
            { "fcmToken", await GetFCMTokenAsync(userId) },
            { "timestamp", Timestamp.GetCurrentTimestamp() }
        };
            await docRef.SetAsync(location, SetOptions.MergeAll);
        }

        public async Task<LocationUpdateModel> GetUserLocationAsync(long userId)
        {
            DocumentReference docRef = _firestoreDb.Collection("users").Document(userId.ToString());
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                double latitude = snapshot.GetValue<double>("latitude");
                double longitude = snapshot.GetValue<double>("longitude");
                return new LocationUpdateModel { Latitude = latitude, Longitude = longitude };
            }
            return null;
        }
        public async Task<List<(double Latitude, double Longitude, string FcmToken)>> GetAllUserLocationsAsync(long excludeUserId)
        {
            var usersRef = _firestoreDb.Collection("users");
            var snapshot = await usersRef.GetSnapshotAsync();
            var userLocations = new List<(double Latitude, double Longitude, string FcmToken)>();

            foreach (var document in snapshot.Documents)
            {
                if (document.Id != excludeUserId.ToString())
                {
                    double latitude = document.GetValue<double>("latitude");
                    double longitude = document.GetValue<double>("longitude");
                    string fcmToken = document.GetValue<string>("fcmToken");
                    userLocations.Add((latitude, longitude, fcmToken));
                }
            }

            return userLocations;
        }
    }
}
