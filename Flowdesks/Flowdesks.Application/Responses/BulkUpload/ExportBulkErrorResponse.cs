using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.BulkUpload;
public class ExportBulkErrorResponse
{

    [DisplayName("Sheet Name")]
    public string? SheetName { get; set; }  

    [Description("Error")]
    public string? Error { get; set; }

    [Description("Record")]
    public string? Record { get; set; }

 
}
