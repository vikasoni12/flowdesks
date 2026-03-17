using AutoMapper;
using Flowdesks.Application.Identity;
using Flowdesks.Application.Requests.Identity;
using Flowdesks.Application.Responses;
using Flowdesks.Application.Responses.Identity;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Infrastructure.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserResponse, ApplicationUser>().ReverseMap();

        //CreateMap<ApplicationUser, UserListResponse>()
        //    .ForMember(x => x.Roles,
        //    src=> src.MapFrom(x => 
        //    x.UserRoles.Select(x => x.Role.Name)))
        //    .ReverseMap();
        CreateMap<PaginatedResult<UserListResponse>, PaginatedResult<ApplicationUser>>().ReverseMap();

        CreateMap<UserViewProfileResponse, ApplicationUser>().ReverseMap();
        CreateMap<UserLoginDeviceHistory, LoginDeviceInfoResponse>().ReverseMap();
        CreateMap<UserPermissionRequest, UserPermission>().ReverseMap();
    }
}
