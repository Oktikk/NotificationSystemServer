using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PraktykiServer.Workers
{
    public class NotificationWorker : BackgroundService
    {
        public NotificationWorker()
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebasekey.json")),
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = "Message title",
                        Body = "Message body"
                    },
                    Topic = "Message"
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var result = await messaging.SendAsync(message);

                Console.WriteLine(result);
                
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

        }
    }
}
