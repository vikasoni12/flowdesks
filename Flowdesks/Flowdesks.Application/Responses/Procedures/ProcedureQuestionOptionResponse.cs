namespace Flowdesks.Application.Responses.Procedures;

public class ProcedureQuestionOptionResponse
{
    public Guid Id { get; set; }
    public Guid ProcedureQuestionId { get; set; }
    public string Option { get; set; }
    public int OrderNumber { get; set; }
}