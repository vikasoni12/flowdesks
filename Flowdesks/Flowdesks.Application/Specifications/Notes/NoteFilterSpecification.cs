using Flowdesks.Application.Requests.Notes;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Note;
using Flowdesks.Shared.Enums;
using Newtonsoft.Json.Linq;

namespace Flowdesks.Application.Specifications.Notes;

public class NoteFilterSpecification : Specification<Note>
{
    public NoteFilterSpecification(NotePagingRequest request)
    {
        if (request.UserId != Guid.Empty && request.UserId != null)
        {
            And(p => p.UserId.Equals(request.UserId));
        }

        if (!string.IsNullOrEmpty(Enum.GetName(typeof(EntityType), request.EntityType)))
        {
            var value = Enum.GetName(typeof(EntityType), request.EntityType);
            And(p => p.EntityType.Equals(value));
        }

        if (!string.IsNullOrEmpty(request.EntityId))
        {
            And(p => p.EntityId.ToString().Equals(request.EntityId));
        }

        if (!string.IsNullOrEmpty(request.CreatedBy))
        {
            And(p => p.CreatedBy.Equals(request.CreatedBy));
        }

        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Text.Contains(request.StringSearch));
        }
    }
}