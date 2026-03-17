using Flowdesks.Application.Attributes;
using System.ComponentModel;

namespace Flowdesks.Application.Requests.Buildings
{
    public class BuildingRequiredFiled
    {
        [Description("Code")]
        [IsRequiredField(true)]
        public string Code { get; set; }

        [Description("Name")]
        [IsRequiredField(true)]
        public string? Name { get; set; }

        [Description("Address")]
        public string? Address { get; set; }

        //[Description("Post Code")]
        //public string? PostCode { get; set; }

        //[Description("Country")]
        //public Guid? CountryId { get; set; }

        [Description("Occupancy Type")]
        public string? OccupancyType { get; set; }

        [Description("Description")]
        public string? Description { get; set; }

        [Description("Site")]
        [IsRequiredField(true)]
        public Guid? SiteId { get; set; }

        [Description("Building Type")]
        public Guid? BuildingTypeId { get; set; }


        [Description("Building schedules")]
        public string? BuildingDailySchedules { get; set; }
    }
}
