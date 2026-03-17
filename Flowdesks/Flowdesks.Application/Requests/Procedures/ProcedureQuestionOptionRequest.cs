namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureQuestionOptionRequest
{
    public Guid? Id { get; set; }
    public Guid? ProcedureQuestionId { get; set; }
    public string Option { get; set; }
    public int OrderNumber { get; set; }
}