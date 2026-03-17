using Flowdesks.Application.Models.Email;

namespace Flowdesks.Application.Interfaces.Email;

public interface IEmailSender
{
    void SendEmail(EmailMessage email);
}
