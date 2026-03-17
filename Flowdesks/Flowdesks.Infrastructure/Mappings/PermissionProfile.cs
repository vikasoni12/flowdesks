using AutoMapper;
using Flowdesks.Application.Responses.Identity;
using Flowdesks.Domain.Entities.Identity;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Infrastructure.Mappings
{
    public class PermissionProfile : Profile
    {
        public PermissionProfile()
        {
            CreateMap<PermissionsResponse, Permission>().ReverseMap();
            CreateMap<PaginatedResult<Permission>, PaginatedResult<PermissionsResponse>>();
        }
    }
}
