using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Documents;
using Flowdesks.Shared.Enums;

namespace Flowdesks.Application.Specifications.Documents;

public class DocumentFilterSpecification : Specification<Document>
{
    public DocumentFilterSpecification(DocumentPagingRequest request)
    {
        if (!String.IsNullOrEmpty(request.CreatedBy))
        {
            And(p => p.CreatedBy.Equals(request.CreatedBy));
        }

        if (!string.IsNullOrEmpty(request.DocumentType))
        {
            And(p => p.DocumentType.Equals(request.DocumentType));
        }

        if (!string.IsNullOrEmpty(request.EntityId))
        {
            And(p => p.EntityId.ToString().Equals(request.EntityId));
        }

        if (!string.IsNullOrEmpty(Enum.GetName(typeof(EntityType), request.EntityType)))
        {
            var value = Enum.GetName(typeof(EntityType), request.EntityType);
            And(p => p.EntityType.Equals(value));
        }

        if (!String.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Title.Contains(request.StringSearch)
            || p.DocumentType.Contains(request.StringSearch));
        }
    }
}
