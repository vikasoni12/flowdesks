namespace Flowdesks.Application.Responses.WorkOrder.Priority
{
    public class PriorityResponse
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int Minutes { get; set; }
        public int TotalHours
        {
            get
            {
                return Minutes / 60;
            }
        }
        public int TotalMinutes
        {
            get
            {
                if (Minutes > 0)
                {
                    int minutes = Minutes % 60;
                    return minutes;
                }
                return 0;
            }
        }
        public string Time
        {
            get
            {
                int hours = TotalHours;
                int minutes = TotalMinutes;

                string time = hours > 0 ? $"{hours} hours" : "";
                time += hours > 0 && minutes > 0 ? " " : "";
                time += minutes > 0 ? $"{minutes} minutes" : "";

                if (time == "")
                {
                    time = "0 minutes";
                }

                return time;
            }
        }
        public int RankOrder { get; set; }
        public int WorkOrderCount {  get; set; }
        public int PPMCount {  get; set; }
    }
}