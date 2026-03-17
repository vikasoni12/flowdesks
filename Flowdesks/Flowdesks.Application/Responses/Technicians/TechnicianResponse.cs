namespace Flowdesks.Application.Responses.Technicians
{
    public class TechnicianResponse
    {
        public Guid Id { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public bool? IsExternalResource { get; set; }       
        public string TimeZone { get; set; }
        public double? StandardHourlyRate { get; set; }
        public string? ImageUrl { get; set; }
        public string? SupplierName { get; set; }
        public string? SupplierId { get; set; }

        private string _status;
        public string Status
        {
            get { return _status ?? "Active"; }
            set { _status = value; }
        }

        public ICollection<TechnicianWorkingDaysResponse>? WorkingDays { get; set; }
        public ICollection<TechnicianSiteResponse>? TechnicianSites { get; set; }
        public ICollection<TechnicianBuildingResponse>? TechnicianBuildings { get; set; }
        public ICollection<TechnicianSkillResponse>? TechnicianSkills { get; set; }
        public ICollection<TechnicianQualificationResponse>? TechnicianQualifications { get; set; }
    }
}
