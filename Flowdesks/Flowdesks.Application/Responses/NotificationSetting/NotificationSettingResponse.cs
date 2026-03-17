namespace Flowdesks.Application.Responses.NotificationSetting
{
    public class NotificationSettingResponse
    {

        public Guid? Id { get; set; }
        public bool? IsAllAssignWorkOrders { get; set; }
        public bool? IsStatusChange { get; set; }
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
