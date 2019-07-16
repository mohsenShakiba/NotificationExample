using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CassandraExample.Domain.AggregatesModel.TokenAggregate;
using CassandraExample.Domain.Exceptions;

namespace CassandraExample.API.DTOs
{
    public class RegisterTokenDTO: IValidatableObject
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        public float Version { get; set; }
        [Required]
        public string AppName { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public string Device { get; set; }

        public DeviceTypes DeviceTypes
        {
            get
            {
                switch (Device.ToLower())
                {
                    case "android": return DeviceTypes.Android;
                    case "ios": return DeviceTypes.IOS;
                    default:
                        throw new InvalidDeviceException();
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validDeviceTypes = new[] {"android", "ios", "web"};
            if (validDeviceTypes.Contains(Device.ToLower()))
            {
                return new ValidationResult[0];
            }
            return new ValidationResult[1] {new ValidationResult("Invalid device type")};
        }
    }
}