using Flowdesks.Domain.Entities.PPMs;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.PPMs
{
    public class AddPPMNoteRequest : CreateEditRequest<PPMNote>, IRequest<Result<int>>
    {
        public Guid? UserId { get; set; }
        public string Text { get; set; }
        public Guid PPMId { get; set; }
        public DateTime RaisedDate { get; set; }
    }
}
