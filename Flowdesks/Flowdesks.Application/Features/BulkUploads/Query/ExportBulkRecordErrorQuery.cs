using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Responses.BulkUpload;
using Flowdesks.Domain.Entities.BulkUpload;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Flowdesks.Application.Features.Technicians.Index.Query.Export
{
    public class ExportBulkRecordErrorQuery : BulkUploadErrorRequest, IRequest<Result<List<ExportBulkErrorResponse>>>
    {
    }

    public class ExportBulkRecordErrorQueryHandler : IRequestHandler<ExportBulkRecordErrorQuery, Result<List<ExportBulkErrorResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _userService;

        public ExportBulkRecordErrorQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<List<ExportBulkErrorResponse>>> Handle(ExportBulkRecordErrorQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var errorLog = await _unitOfWork.Repository<ImportFileErrorLog>().Entities().Where(x=>x.FileCode.Equals( new Guid( request.FileCode))).OrderBy(x=>x.SheetName).ProjectTo<ExportBulkErrorResponse>(_mapper.ConfigurationProvider) .ToListAsync(cancellationToken);

                return await Result<List<ExportBulkErrorResponse>>.SuccessAsync(data: errorLog);
            }
            catch (Exception ex)
            {
                return Result<List<ExportBulkErrorResponse>>.Fail(ex.Message);
            }
        }
    }
}
