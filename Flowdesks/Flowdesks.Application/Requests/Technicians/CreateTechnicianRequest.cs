using Flowdesks.Application.Attributes;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Technicians
{
    public class CreateTechnicianRequest : CreateEditRequest<Domain.Entities.Technicians.Technician>, IRequest<Result<int>>
    {
        [Description("Id number")]
        [IsRequiredField(true)]
        public string IdNumber { get; set; }
        [IsRequiredField(true)]
        public string Name { get; set; }
      
        [Description("Mobile Number")]
        public string MobileNumber { get; set; }
        [IsRequiredField(true)]
        public string Email { get; set; }

        [Description("Start Date")]
        public DateTime? StartDate { get; set; }

        [Description("Finish Date")]
        public DateTime? FinishDate { get; set; }

        [Description("Is External Resource")]
        public bool? IsExternalResource { get; set; }
        public string TimeZone { get; set; }
      
        [Description("Standard Hourly Rate")]
        public double? StandardHourlyRate { get; set; }
        public string Status { get; set; } = UserStatus.Active.ToString();
      
        [Description("Supplier")]
        public Guid? SupplierId { get; set; }
        
        [Description("Profile Picture")]
        public UploadByteArray? ProfilePicture { get; set; }

        [Description("Working days")]
        public List<DayOfWeek>? WorkingDaysIds { get; set; }
      
        [Description("Sites")]
        public List<Guid>? SiteIds { get; set; }
      
        [Description("Buildings")]
        public List<Guid>? BuildingIds { get; set; }
      
        [Description("Skills")]
        [IsRequiredField(true)]
        public List<Guid>? SkillIds { get; set; }
       
        [Description("Qualifications")]
        public List<Guid>? QualificationIds { get; set; }
    }
}