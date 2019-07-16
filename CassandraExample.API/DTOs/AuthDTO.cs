using System.ComponentModel.DataAnnotations;

namespace CassandraExample.API.DTOs
{
    public class AuthDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}