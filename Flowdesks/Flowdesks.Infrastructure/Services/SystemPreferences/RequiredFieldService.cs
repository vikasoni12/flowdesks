using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.SystemPreferences;
using Flowdesks.Domain.Entities.SystemPreferences;

namespace Flowdesks.Infrastructure.Services.SystemPreferences;

public class RequiredFieldService : IRequiredFieldService
{
    private readonly IUnitOfWork _unitOfWork;

    public RequiredFieldService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public List<string> GetFieldsByEntityType(string entityType)
    {
        return _unitOfWork.Repository<EntityRequiredField>().Entities()
            .Where(x => x.EntityType == entityType)
            .Select(x => x.Value).ToList();
    }
}
