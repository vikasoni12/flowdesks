using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Buildings;

public class AddUpdateBuildingTypeRequest : IRequest<Result<int>>
{
    public string Name { get; set; }
}