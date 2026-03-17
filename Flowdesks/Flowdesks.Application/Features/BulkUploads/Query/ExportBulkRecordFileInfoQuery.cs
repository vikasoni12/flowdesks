using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Features.Technicians.Index.Query.Export;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.BulkUpload;
using Flowdesks.Application.Responses.BulkUpload;
using Flowdesks.Application.Responses.Permit;
using Flowdesks.Domain.Entities.BulkUpload;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Features.BulkUploads.Query;
public class ExportBulkRecordFileInfoQuery : BulkUploadFileDetailRequest, IRequest<Result<BulkUploadFileCodeResponse>>
{
}

public class ExportBulkRecordFileInfoQueryHandler : IRequestHandler<ExportBulkRecordFileInfoQuery, Result<BulkUploadFileCodeResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _userService;

    public ExportBulkRecordFileInfoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService userService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<Result<BulkUploadFileCodeResponse>> Handle(ExportBulkRecordFileInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var current=_userService.UserId;

          var code= await _unitOfWork.Repository<ImportFileDetail>().Entities().Where(x => x.CreatedBy.Equals(current) && x.HasError==true).OrderByDescending(x=>x.CreatedOn).ProjectTo<BulkUploadFileCodeResponse>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return  Result<BulkUploadFileCodeResponse>.Success(code);
        }
        catch (Exception ex)
        {
            return Result<BulkUploadFileCodeResponse>.Fail(ex.Message);
        }
    }
}


