using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Shared.Wrapper;

namespace Flowdesks.Application.Interfaces.Common;
public interface IDuplicateRecordsForBulkService
{
    Task<bool> CheckBuildingCode(string code); 
    Task<bool> CheckSiteCode(string code);
    Task<bool> CheckSupplierCode(string code);
    Task<bool> CheckStockPartCode(string code);
    Task<bool> CheckTechnicianIdNumber(string code);
  
}
