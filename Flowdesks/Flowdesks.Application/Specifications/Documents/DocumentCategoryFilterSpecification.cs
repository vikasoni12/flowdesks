using Flowdesks.Application.Requests.Documents;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Documents;

namespace Flowdesks.Application.Specifications.Documents;

public class DocumentCategoryFilterSpecification : Specification<DocumentCategory>
{
    public DocumentCategoryFilterSpecification(DocumentCategoryPagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch));
        }
    }
}
