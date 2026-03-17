using Flowdesks.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Domain.Entities.BulkUpload;
public class ImportFileErrorLog:FullAuditableEntity<Guid>
{
    public string SheetName {  get; set; }
    public string? Record { get; set;}
    public string? Error { get; set;}
    public Guid? FileCode {  get; set; }

}
