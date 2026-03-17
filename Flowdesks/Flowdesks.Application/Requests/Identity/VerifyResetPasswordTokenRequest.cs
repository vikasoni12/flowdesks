using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Application.Requests.Identity;

public class VerifyResetPasswordTokenRequest
{
    [Required] public string UserId { get; set; }
    [Required] public string Token { get; set; }
}
