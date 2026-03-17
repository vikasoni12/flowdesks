namespace Flowdesks.Application.Requests.Procedures;

public class ProcedureQuestionRequest
{
    public Guid? Id { get; set; }
    public Guid? ProcedureId { get; set; }
    public Guid? SectionId { get; set; }
    public int OrderNumber { get; set; }
    public string FieldName { get; set; }
    public string? QuestionType { get; set; }
    public bool IsRequired { get; set; }
    public virtual ICollection<ProcedureQuestionOptionRequest>? ProcedureQuestionOptions { get; set; }
}