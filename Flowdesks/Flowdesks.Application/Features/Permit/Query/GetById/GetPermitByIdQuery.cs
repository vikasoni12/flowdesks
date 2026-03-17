using AutoMapper;
using AutoMapper.QueryableExtensions;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Responses.Permit;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PermitEntity = Flowdesks.Domain.Entities.Permit.Permit;

namespace Flowdesks.Application.Features.Permit.Query.GetById;

public class GetPermitByIdQuery : IRequest<Result<PermitResponse>>
{
    public Guid Id { get; set; }
}

public class GetPermitByIdQueryHandler : IRequestHandler<GetPermitByIdQuery, Result<PermitResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetPermitByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PermitResponse>> Handle(GetPermitByIdQuery request, CancellationToken cancellationToken)
    {
        var permit = await _unitOfWork.Repository<PermitEntity>().Entities().ProjectTo<PermitResponse>(_mapper.ConfigurationProvider)
           .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        return await Result<PermitResponse>.SuccessAsync(permit);
    }
}