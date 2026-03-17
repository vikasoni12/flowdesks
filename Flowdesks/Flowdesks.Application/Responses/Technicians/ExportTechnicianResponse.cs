using System.ComponentModel;

namespace Flowdesks.Application.Responses.Technicians
{
    public class ExportTechnicianResponse
    {
        [Description("Id Number")]
        public string IdNumber { get; set; }
        [Description("Name")]
        public string Name { get; set; }
        [Description("Mobile Number")]
        public string MobileNumber { get; set; }
        [Description("Email")]
        public string Email { get; set; }
        [Description("Start Date")]
        public DateTime? StartDate { get; set; }
        [Description("Finish Date")]
        public DateTime? FinishDate { get; set; }
        [Description("External Resource")]
        public bool? IsExternalResource { get; set; }
        [Description("Time Zone")]
        public string TimeZone { get; set; }
        [Description("Standard Hourly Rate")]
        public double? StandardHourlyRate { get; set; }
    }
}
