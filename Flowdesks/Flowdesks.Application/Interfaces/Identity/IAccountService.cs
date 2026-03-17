using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Requests.Identity;
using Flowdesks.Application.Validators.Identity;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Interfaces.Identity;

public interface IAccountService : IService
{
    Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, Guid userId);

    Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

    Task<IResult<string>> GetProfilePictureAsync(string userId);

    Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, Guid userId);
}