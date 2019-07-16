using System.ComponentModel.DataAnnotations;

namespace CassandraExample.API.DTOs
{
    public class SendNotificationDTO
    {
        [Required] public string Title { get; set; }

        public string Message { get; set; }

        [Required] public string PhoneNumber { get; set; }

        [Required] public string AppName { get; set; }
    }
}