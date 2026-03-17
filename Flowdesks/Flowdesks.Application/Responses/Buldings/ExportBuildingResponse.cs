using System.ComponentModel;

namespace Flowdesks.Application.Responses.Buldings
{
    public class ExportBuildingResponse
    {
        [Description("Code")]
        public string Code { get; set; }

        [Description("Name")]
        public string? Name { get; set; }

        [Description("Address")]
        public string? Address { get; set; }

        [Description("Description")]
        public string? Description { get; set; }

        [Description("Site")]
        public string? Site { get; set; }

        [Description("Building Type")]
        public string? BuildingType { get; set; }

       
    }
}
