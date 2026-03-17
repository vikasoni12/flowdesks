using Flowdesks.Domain.Common;
using Flowdesks.Domain.Entities.BulkUpload;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.BulkUpload;
public class BulkUploadErrorLogResponse
{
        public Guid? FileCode { get; set; }
        public string SheetName { get; set; }
        public string? Record { get; set; }
        public string? Error { get; set; }
    

}
