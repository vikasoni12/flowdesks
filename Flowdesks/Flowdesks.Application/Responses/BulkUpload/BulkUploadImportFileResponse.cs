using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.BulkUpload;
public class BulkUploadImportFileResponse
{
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? FileCode {get;set;}
    public Boolean? HasError {  get; set; }
}
