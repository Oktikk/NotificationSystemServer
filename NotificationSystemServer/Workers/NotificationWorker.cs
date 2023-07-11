﻿using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NotificationSystemServer.Data;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NotificationSystemServer.Workers
{
    public class NotificationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationWorker(IServiceProvider serviceProvider)
        {
            var defaultApp = FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebasekey.json")),
            });
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var deviceTokens = await GetDeviceTokensFromDatabase();

                await SendNotificationsToDeviceTokens(deviceTokens, RandomString(20), RandomString(40));

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }

        }

        private async Task<List<string>> GetDeviceTokensFromDatabase()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var dbContext = scopedProvider.GetRequiredService<ClientDbContext>();
                var devices = dbContext.Clients.Select(t => t.FCMToken).ToList();
                return devices;
            }
        }

        private async Task SendNotificationsToDeviceTokens(List<string> deviceTokens, string title, string body)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

            var tasks = new List<Task>();
            foreach (var token in deviceTokens)
            {
                var message = new Message()
                {
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    },
                    Token = token
                };

                tasks.Add(messaging.SendAsync(message));
            }

            await Task.WhenAll(tasks);
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}