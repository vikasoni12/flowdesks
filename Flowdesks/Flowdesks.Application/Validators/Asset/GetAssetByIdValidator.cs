using Flowdesks.Application.Requests.Asset;
using FluentValidation;

namespace Flowdesks.Application.Validators.Asset;

public class GetAssetByIdValidator : AbstractValidator<GetAssetByIdRequest>
{
    public GetAssetByIdValidator()
    {
        RuleFor(x => x.AssetId).NotNull().NotEmpty();
    }
}
