using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.WorkOrder.Category;

public class AddUpdateCategoryRequest : IRequest<Result<int>>
{
    public string Name { get; set; }
}
