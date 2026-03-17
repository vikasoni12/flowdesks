using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Buildings;
using Flowdesks.Domain.Entities.Sites;
using Flowdesks.Shared.Wrapper;
using System.Threading;

namespace Flowdesks.Infrastructure.Services.Common;
public class DuplicateRecordsForBulkService : IDuplicateRecordsForBulkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DuplicateRecordsForBulkService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
   
   
   
    
   

   
}
