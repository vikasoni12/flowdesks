using Flowdesks.Domain.Entities.Notifications;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.NotificationSettings
{
    public class CreateUpdateNotificationSettingRequest : CreateEditRequest<NotificationSetting>, IRequest<Result<int>>
    {
        public bool? IsAllAssignWorkOrders { get; set; }
        public bool? IsOverDue { get; set; }
        public bool? IsStatusChange { get; set; }
        public bool? IsOverDueCreatedByMe { get; set; }
        public bool? IsStatusChangeCreatedByMe { get; set; }
        public bool? IsRequestUnassigned { get; set; }
        public bool? IsRequiringApproval { get; set; }
        public bool? IsAllAssignForSite { get; set; }
        public bool? IsAllAssignOverDue { get; set; }
        public bool? IsAllAssignNewNote { get; set; }
        public bool? IsAllCreatedByMeForSite { get; set; }
        public bool? IsAllCreatedByMeOverDue { get; set; }
        public bool? IsAllCreatedByMeNewNote { get; set; }
        public bool? IsPurchaseOrderApproved { get; set; }
        public bool? IsPurchaseOrderRejected { get; set; }
        public bool? IsWorkRequestCreated { get; set; }
        public bool? IsWorkRequestStatusChanges { get; set; }
        public bool? IsFireCertification { get; set; }
        public bool? IsSupplierInsuranceExpired { get; set; }
        public bool? IsTechnicianQualificationUpdated { get; set; }
        public bool? IsQuoteReceived { get; set; }
        public string? MessageType { get; set; }

        public Guid? UserId { get; set; }
    }
}
