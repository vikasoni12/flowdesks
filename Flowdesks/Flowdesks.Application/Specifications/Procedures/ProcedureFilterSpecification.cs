using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.Procedure;

namespace Flowdesks.Application.Specifications.Procedures;

public class ProcedureFilterSpecification : Specification<Procedure>
{
    public ProcedureFilterSpecification(ProcedurePagingRequest request)
    {
        if (!string.IsNullOrEmpty(request.Id))
        {
            And(p => p.Id.ToString() == request.Id);
        }

        if (request.AssetTypeIds != null && request.AssetTypeIds.Count > 0)
        {
            And(p => request.AssetTypeIds.Contains((Guid)p.AssetTypeId));
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            And(p => p.Name.Contains(request.Name));
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            And(p => p.Category.Contains(request.Category));
        }

        if (!string.IsNullOrEmpty(request.EntityId))
        {
            And(p => p.ProcedureMappings.Any(pm => pm.EntityId.ToString() == request.EntityId));
        }

        if (!String.IsNullOrEmpty(request.StringSearch))
        {
            And(p => p.Name.Contains(request.StringSearch)
            || p.AssetType.Name.Contains(request.StringSearch)
            || p.Description.Contains(request.StringSearch));
        }
    }
}