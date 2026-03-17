using Flowdesks.Application.Responses.Technicians;
using Flowdesks.Domain.Entities.Technicians;
using Flowdesks.Shared.Enums;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Technicians
{
    public class UpdateTechnicianRequest : CreateEditRequest<Technician>, IRequest<Result<TechnicianResponse>>
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
        public string ImageUrl { get; set; }
        public string Status { get; set; } = UserStatus.Active.ToString();
        public UploadByteArray? ProfilePicture { get; set; }
        public Guid? SupplierId { get; set; }
        public List<DayOfWeek> WorkingDaysIds { get; set; }
        public List<Guid>? SiteIds { get; set; }
        public List<Guid>? BuildingIds { get; set; }
        public List<Guid>? SkillIds { get; set; }
        public List<Guid>? QualificationIds { get; set; }
    }
}