using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.DTOs
{
    public class UserDto
    {
        [Required]
        [ForeignKey("UserId")]
        public string UserId { get; set; } = null!;

        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^\s@]+@[^\s@]+.[^\s@]{2,}$", ErrorMessage = "Email invalid.")]
        public string Email { get; set; } = null!;

        [ProtectedPersonalData]
        public string? FirstName { get; set; }

        [ProtectedPersonalData]
        public string? LastName { get; set; }

        [ProtectedPersonalData]
        public string? ProfileImageUrl { get; set; }

        [ProtectedPersonalData]
        public string? Gender { get; set; }

        [ProtectedPersonalData]
        public int Age { get; set; }
    }
}
