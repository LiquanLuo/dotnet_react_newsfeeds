using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,20}$", ErrorMessage = "Passoword must contain digits, lowercase and uppercase Letters, 4-20 characters")]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }
        
        [Required]
        public string Username { get; set; }
    }
}