using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.BulkUpload;
public class BulkUploadDatatableResponse
{
    public DataTable? DataTable { get; set; }
    public string SheetName { get; set; }
    public string Error { get; set; }

}
public class BulkUploadGenericDatatableResponse<T>
{
    public List<T> Data { get; set; } = new List<T>();
    public string SheetName { get; set; }
    public string Error { get; set; }
}
