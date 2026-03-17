using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Master;
using Flowdesks.Application.Responses.Master;
using MediatR;

namespace Flowdesks.Application.Features.Master.Country
{
    public class GetAllCountryListQuery : IRequestHandler<GetAllCountryRequest,List<CountryDto>>
    {  
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCountryListQuery(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<List<CountryDto>> Handle(GetAllCountryRequest request, CancellationToken cancellationToken)
        {
            var countryList = _unitOfWork.Repository<Domain.MasterEntities.Country>().GetAll();
            return _mapper.Map<List<CountryDto>>(countryList);
        }
    }
}
