using System.ComponentModel.DataAnnotations;
using CassandraExample.API.DTOs;
using MediatR;

namespace CassandraExample.API.Application.Commands
{
    public class LoginCommand: IRequest<LoginResultDTO>
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password { get; set; }
    }
}