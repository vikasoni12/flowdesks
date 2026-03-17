using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Permit;
using Flowdesks.Shared.Wrapper;
using MediatR;
using PermitEntity = Flowdesks.Domain.Entities.Permit.Permit;

namespace Flowdesks.Application.Features.Permit.Command.Add;

public class AddPermitCommand : IRequestHandler<AddPermitRequest, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddPermitCommand(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(AddPermitRequest request, CancellationToken cancellationToken)
    {
        if (_unitOfWork.Repository<PermitEntity>().Entities().Any(x => x.Name.Equals(request.Name, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<int>.Fail("Permit with same name already exists");
        }

        var permit = _mapper.Map<PermitEntity>(request);

        _unitOfWork.Repository<PermitEntity>().Add(permit);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result<int>.Success();
    }
}
