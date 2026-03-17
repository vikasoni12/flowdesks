namespace Flowdesks.Application.Responses.WorkOrder.SlaSetting
{
    public class SlaSettingResponse
    {
        public Guid? Id { get; set; }
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
        public string Color { get; set; }
    }
}
