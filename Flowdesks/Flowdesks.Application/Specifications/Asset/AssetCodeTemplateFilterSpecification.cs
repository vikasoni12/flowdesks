using Flowdesks.Application.Requests.Asset;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.SystemPreferences.Asset;

namespace Flowdesks.Application.Specifications.Asset
{
    public class AssetCodeTemplateFilterSpecification : Specification<AssetTemplateCode>
    {
        public AssetCodeTemplateFilterSpecification(AssetCodeTemplatePagingRequest request)
        {
            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.FieldName.Contains(request.StringSearch));
            }
        }
    }
}
