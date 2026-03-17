namespace Flowdesks.Application.Interfaces.SystemPreferences
{
    public interface IRequiredFieldService
    {
        List<string> GetFieldsByEntityType(string entityType);
    }
}
