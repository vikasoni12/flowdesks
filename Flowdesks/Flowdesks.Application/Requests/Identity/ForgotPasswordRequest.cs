using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Application.Requests.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}