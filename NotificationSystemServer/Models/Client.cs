using System.ComponentModel.DataAnnotations;

namespace NotificationSystemServer.Models
{
    public class Client
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string FCMToken { get; set; }
    }
}
