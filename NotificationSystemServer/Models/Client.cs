using System.ComponentModel.DataAnnotations;

namespace PraktykiServer.Models
{
    public class Client
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public Guid guid { get; set; }
    }
}
