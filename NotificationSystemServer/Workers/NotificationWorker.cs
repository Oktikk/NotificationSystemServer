namespace NotificationSystemServer.Workers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using FirebaseAdmin;
    using FirebaseAdmin.Messaging;
    using Google.Apis.Auth.OAuth2;
    using Microsoft.Extensions.DependencyInjection;
    using NotificationSystemServer.Data;

    public class NotificationWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public NotificationWorker(IServiceProvider serviceProvider)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebasekey.json")),
            });
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var deviceTokens = this.GetDeviceTokensFromDatabase();

                await this.SendNotificationsToDeviceTokens(deviceTokens, GetRandomNotificationType());

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }

        private async Task SendNotificationsToDeviceTokens(List<string?> deviceTokens, Notification notification)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            var tasks = new List<Task>();
            foreach (var token in deviceTokens)
            {
                var message = new Message()
                {
                    Notification = notification,
                    Token = token,
                };

                tasks.Add(messaging.SendAsync(message));
            }

            await Task.WhenAll(tasks);
        }

        private List<string?> GetDeviceTokensFromDatabase()
        {
            using (var scope = this.serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var dbContext = scopedProvider.GetRequiredService<ClientDbContext>();
                var devices = dbContext.Clients.Select(t => t.FCMToken).ToList();
                return devices;
            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static Notification GetRandomNotificationType()
        {
            int notificationType = random.Next(0, 2);
            string title = string.Empty;
            string body = string.Empty;

            switch (notificationType)
            {
                case 0:
                    title = "Clipboard";
                    body = RandomString(40);
                    break;
                case 1:
                    title = RandomString(20);
                    body = RandomString(40);
                    break;
            }

            return new Notification()
            {
                Title = title,
                Body = body,
            };
        }
    }
}
