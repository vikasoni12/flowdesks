namespace Flowdesks.Application.Interfaces.Email.IEmailPopulate
{
    public interface IEmailPopulateBody
    {
        Task<string> PopulateBody(string emailType);
    }
}
