namespace Flowdesks.Application.Requests.Buildings;

public class BuildingDailyScheduleRequest
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpenFrom { get; set; }
    public TimeSpan OpenTo { get; set; }
}
