using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.RequiredFields
{
    public class AddRequiredFieldsRequest : IRequest<Result<int>>
    {
        public EntityType EntityType { get; set; }
        public List<FieldRequest> Fields { get; set; }
    }

    public class FieldRequest
    {
        public string Title { get; set; }
        public string Value { get; set; }
    }
}
