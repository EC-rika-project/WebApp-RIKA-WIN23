using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DTOs
{
    public class UserAddressDto
    {
        [Required]
        public string UserId { get; set; } = null!;

        [ProtectedPersonalData]
        public string? AddressLine { get; set; }

        [ProtectedPersonalData]
        public string? ApartmentNumber { get; set; }

        [RegularExpression(@"^\d{5}$", ErrorMessage = "PostalCode invalid, submit 5 digits only.")]
        public int PostalCode { get; set; } = 00000;

        public string? City { get; set; }

        public string? Country { get; set; }
    }
}
