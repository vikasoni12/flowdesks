using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Permit;
using Flowdesks.Shared.Wrapper;
using MediatR;
using PermitEntity = Flowdesks.Domain.Entities.Permit.Permit;

namespace Flowdesks.Application.Features.Permit.Command.Update;

public class UpdatePermitCommand : AddPermitRequest
{
    public Guid Id { get; set; }
}

public class UpdatePermitCommandHandler : IRequestHandler<UpdatePermitCommand, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdatePermitCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(UpdatePermitCommand request, CancellationToken cancellationToken)
    {
        var permit = await _unitOfWork.Repository<PermitEntity>().GetByIdAsync(request.Id);

        if (permit != null)
        {
            _mapper.Map<AddPermitRequest, PermitEntity>(request, permit);

            _unitOfWork.Repository<PermitEntity>().Update(permit);

            await _unitOfWork.SaveAsync(cancellationToken);

            return Result<int>.Success("Permit updated successfully");
        }
        else
        {
            return Result<int>.Fail($"Permit with {request.Id} not found");
        }
    }
}