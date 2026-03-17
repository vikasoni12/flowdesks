using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Contacts
{
    public class AddUpdateContactClassRequest : IRequest<Result<int>>
    {
        public string Name { get; set; }
    }
}
