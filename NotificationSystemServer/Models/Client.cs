namespace NotificationSystemServer.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Client
    {
        [Required]
        public int ID { get; set; }

        [Required]
        public string? FCMToken { get; set; }
    }
}
