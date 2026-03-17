using AutoMapper;
using Flowdesks.Application.Interfaces.Common;
using Flowdesks.Application.Interfaces.Identity;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Interfaces.User;
using Flowdesks.Application.Requests.Identity;
using Flowdesks.Application.Requests.Tenant;
using Flowdesks.Application.Validators.Tenant;
using Flowdesks.Shared.Constants.User;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using static Flowdesks.Shared.Constants.Common.ApplicationConstants;
using TenantEntity = Flowdesks.Domain.Entities.Tenant.Tenant;

namespace Flowdesks.Application.Features.Tenant.Command.Create;

public class CreateTenantCommand : IRequestHandler<CreateTenantRequest, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IDatabaseSeeder _databaseSeeder;
    private readonly IHttpContextAccessor _httpContext;
    private readonly ICurrentUserService _currenUserService;

    public CreateTenantCommand(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, IDatabaseSeeder databaseSeeder, IHttpContextAccessor httpContext, ICurrentUserService currenUserService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userService = userService;
        _databaseSeeder = databaseSeeder;
        _httpContext = httpContext;
        _currenUserService=currenUserService;
    }

    public async Task<Result<string>> Handle(CreateTenantRequest request, CancellationToken cancellationToken)
    {
        var tenantValidator = new CreateTenantValidator(_unitOfWork);
        var validationResult = await tenantValidator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
            return Result<string>.Fail(errors);
        }

        var isUserExist = await _userService.IsUserDetailAlreadyExist(request.Email, request.PhoneNumber);
        if (!isUserExist.Succeeded)
        {
            return Result<string>.Fail(isUserExist.Messages);
        }

        var tenant = _mapper.Map<TenantEntity>(request);
        tenant.Name = GetTenantName(request.Email);

        _unitOfWork.Repository<TenantEntity>().Add(tenant);

        await _unitOfWork.SaveAsync(cancellationToken);

        _httpContext.HttpContext.Items["TenantId"] = tenant.Id;

        // Seed data for the new tenant
        _databaseSeeder.Initialize();

        string password = _userService.CreatePassword();
        var userRequest = new RegisterRequest
        {
            TenantId = tenant.Id,
            Email = request.Email,
            FirstName=request.FirstName,
            LastName=request.LastName,
            Password = password,
            Roles = new List<string> { RoleConstants.AdministratorRole },
            Origin = _currenUserService.OriginUrl,
        };

        var registerResult = await _userService.RegisterUser(userRequest);

        if (!registerResult.Succeeded)
            return Result<string>.Fail(registerResult.Messages);

        return Result<string>.Success(MessageConstants.UserCreated);
    }
    public static string GetTenantName(string email)
    {
        // Regular expression to match the domain part between '@' and '.'
        Match match = Regex.Match(email, @"@([^.]+)\.");
        return match.Success ? match.Groups[1].Value : string.Empty;
    }
}