using Flowdesks.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Domain.Entities.BulkUpload;
public class ImportFileDetail : FullAuditableEntity<Guid>
{
    public Guid? FileCode {  get; set; }
    public string Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Boolean? HasError {  get; set; }
}
