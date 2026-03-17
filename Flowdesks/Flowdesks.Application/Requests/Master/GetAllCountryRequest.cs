using Flowdesks.Application.Responses.Master;
using MediatR;

namespace Flowdesks.Application.Requests.Master
{
    public class GetAllCountryRequest:IRequest<List<CountryDto>>
    {
    }
}
