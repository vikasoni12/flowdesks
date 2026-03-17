using Flowdesks.Application.Requests.Teams;
using Flowdesks.Shared.Constants.User;
using System.ComponentModel.DataAnnotations;

namespace Flowdesks.Application.Requests.Identity;

public class UpdateUserRequest
{
    public Guid? Id { get; set; }
    [Required] 
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string UserLoginType { get; set; } = UserLoginTypeConstants.Database;
    public string RoleName { get; set; }
    public UploadByteArray? ProfilePicture { get; set; } = new();
    public string? Origin { get; set; }

    public List<TeamUserRequest>? TeamUsers { get; set; }
    public List<UserSiteRequest> UserSites { get; set; }
    public List<UserBuildingRequest> UserBuildings { get; set; }
}
