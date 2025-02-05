using Google.Apis.Auth.OAuth2;
using Google.Apis.FirebaseCloudMessaging.v1;
using Google.Apis.FirebaseCloudMessaging.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class FCMService
{
    private readonly FirebaseCloudMessagingService _fcmService;

    public FCMService(string credentialsFilePath)
    {
        var credential = GoogleCredential.FromFile(credentialsFilePath)
            .CreateScoped(FirebaseCloudMessagingService.ScopeConstants.CloudPlatform);

        _fcmService = new FirebaseCloudMessagingService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential
        });
    }

    public async Task SendNotificationAsync(string target, string title, string body, IDictionary<string, string> data = null)
    {
        var message = new Message
        {
            Notification = new Notification
            {
                Title = title,
                Body = body
            },
            Data = data ?? new Dictionary<string, string>(),
            Token = target 
        };

        try
        {
            var request = new SendMessageRequest
            {
                Message = message
            };
            var response = await _fcmService.Projects.Messages.Send(request, "projects/triple-grove-413413").ExecuteAsync();
            Console.WriteLine($"Successfully sent notification to {target}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send notification to {target}: {ex.Message}");
        }
    }
}
