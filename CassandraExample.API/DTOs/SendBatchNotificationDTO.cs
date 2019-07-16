using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CassandraExample.API.DTOs
{
    public class SendBatchNotificationDTO
    {
        [Required]
        public string Title { get; set; }
        public string Message { get; set; }
        [Required]
        public IEnumerable<string> PhoneNumbers { get; set; }
        [Required]
        public string AppName { get; set; }
        public string ProjectName { get; set; }
        public NotificationFilterDTO Filter { get; set; }
    }
}