using Flowdesks.Domain.Entities.SystemPreferences.Asset;
using Flowdesks.Shared.Wrapper;
using MediatR;

namespace Flowdesks.Application.Requests.Asset
{
    public class AddUpdateAssetCodeTemplateRequest : CreateEditRequest<AssetTemplateCode>, IRequest<Result<int>>
    {
       public List<CreateAssetTemplateCodeRequest> CreateTemplate {  get; set; }
    }

    public class CreateAssetTemplateCodeRequest
    {
        public Guid? Id { get; set; } 
        public string FieldName { get; set; }
        public int NoOfCharacter { get; set; }
        public bool IsTemplateUse { get; set; }
    }
}
