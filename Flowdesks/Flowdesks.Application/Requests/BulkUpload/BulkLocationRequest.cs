using Flowdesks.Domain.Entities.Buildings;
using Flowdesks.Shared.Wrapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.BulkUpload;
public class BulkLocationRequest : CreateEditRequest<BuildingLocation>, IRequest<Result<int>>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string? Floor { get; set; }
    public Guid BuildingId { get; set; }
}