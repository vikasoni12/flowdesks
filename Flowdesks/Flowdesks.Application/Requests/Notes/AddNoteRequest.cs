using Flowdesks.Domain.Entities.Note;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Notes;

public class AddNoteRequest : CreateEditRequest<Note>, IRequest<Result<int>>
{
    public Guid? UserId { get; set; }
    public string Text { get; set; }
    public EntityType EntityType { get; set; }
    public Guid EntityId { get; set; }
}