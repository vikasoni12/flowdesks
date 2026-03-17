namespace Flowdesks.Application.Responses.WorkOrder.TimeRecord
{
    public class TimeRecordResponse
    {
        public Guid? Id { get; set; }
        public string IdNumber { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? FinishDateTime { get; set; }
        public Guid? TechnicianId { get; set; }
        public string TechnicianName { get; set; }
        public string TotalTime
        {
            get
            {
                if (StartDateTime.HasValue && FinishDateTime.HasValue)
                {
                    TimeSpan duration = FinishDateTime.Value - StartDateTime.Value;
                    return $"{(int)duration.TotalHours} h and {duration.Minutes} min";
                }
                return null;
            }
        }
    }
}
