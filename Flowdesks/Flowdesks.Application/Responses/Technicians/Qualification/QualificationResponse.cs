namespace Flowdesks.Application.Responses.Technicians.Qualification
{
    public class QualificationResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateObtained { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Guid? SkillId { get; set; }
        public string? Skill { get; set; }

        public string TechnicianName { get; set; }
        public string TechnicianIdNumber { get; set; }
    }
}
