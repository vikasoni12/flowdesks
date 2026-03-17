namespace Flowdesks.Application.Responses.Buldings;

public class BuildingDailyScheduleResponse
{
    public DayOfWeek DayOfWeek { get; set; }
    public TimeSpan OpenFrom { get; set; }
    public TimeSpan OpenTo { get; set; }
}