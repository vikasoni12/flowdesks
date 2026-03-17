using AutoMapper;
using Flowdesks.Application.Interfaces.Persistence;
using Flowdesks.Application.Requests.Procedures;
using Flowdesks.Application.Responses.Procedures;
using Flowdesks.Application.Specifications.Procedures;
using Flowdesks.Domain.Entities.PPM;
using Flowdesks.Domain.Entities.Procedure;
using Flowdesks.Domain.Entities.WorkOrder;
using Flowdesks.Infrastructure.Extensions;
using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderEntity = Flowdesks.Domain.Entities.WorkOrder.WorkOrder;

namespace Flowdesks.Application.Features.Procedures.Query.GetAll;

public class GetAllResponsesQuery : IRequestHandler<ProcedureResponsePagingRequest, Result<PaginatedResult<ProcedurePagingResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAllResponsesQuery(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<ProcedurePagingResponse>>> Handle(ProcedureResponsePagingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            ProcedureResponseFilterSpecification spec = new(request);

            var query = _unitOfWork.Repository<Procedure>().Entities().Include(x => x.AssetType).Include(x => x.Questions).ThenInclude(x => x.ProcedureQuestionOptions).Include(x => x.Sections).ThenInclude(x => x.Questions).Include(x => x.ProcedureMappings).ThenInclude(x => x.Responses).OrderByDescending(x => x.CreatedOn).Specify(spec);

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                query = query.ApplySorting(request.SortColumn, request.SortOrder);
            }

            IQueryable<ProcedureMapping> procedureMapping;

            if (request.EntityId != null)
                procedureMapping = _unitOfWork.Repository<ProcedureMapping>().Entities().Where(x => x.EntityId.ToString() == request.EntityId);
            else
                procedureMapping = _unitOfWork.Repository<ProcedureMapping>().Entities();

            List<ProcedurePagingResponse> data;

            if (request.Category == "PPM")
            {
                var ppm = _unitOfWork.Repository<PPM>().Entities().Include(x => x.Technician);

                data = await (from c in query
                              join pM in procedureMapping on c.Id equals pM.ProcedureId into pmg
                              from pg in pmg.DefaultIfEmpty()
                              join p in ppm on pg.EntityId equals p.Id into ppmg
                              from mg in ppmg.DefaultIfEmpty()
                              select new ProcedurePagingResponse()
                              {
                                  Id = c.Id,
                                  Name = c.Name,
                                  AssetTypeId = c.AssetTypeId,
                                  AssetType = c.AssetType != null ? c.AssetType.Name : null,
                                  Category = "PPM",
                                  Frequency = null,
                                  FrequencyOccurrence = 0,
                                  Description = c.Description,
                                  Questions = _mapper.Map<List<ProcedureQuestionResponse>>(c.Questions),
                                  Sections = _mapper.Map<List<ProcedureSectionResponse>>(c.Sections),
                                  ProcedureMappings = new List<ProcedureMappingResponse>
                                   {
                                       new ProcedureMappingResponse()
                                       {
                                           Id = pg != null ? pg.Id : Guid.Empty,
                                           EntityId = mg != null ? mg.Id : Guid.Empty,
                                           EntityType = pg != null ? pg.EntityType : null,
                                           ProcedureId = c.Id,
                                           EntityCode = mg != null ? mg.TaskId : null,
                                           Problem = mg != null ? mg.Problem : null,
                                           TechnicianName = mg != null && mg.Technician != null ? mg.Technician.Name : null,
                                           Responses = pg != null ? _mapper.Map<List<Responses.Procedures.ProcedureResponse>>(pg.Responses) : new List<Responses.Procedures.ProcedureResponse>()
                                       }
                                   }
                              }).ToListAsync();
            }
            else
            {
                var workOrder = _unitOfWork.Repository<WorkOrderEntity>().Entities().Include(x => x.Technician);

                var workOrderProcedure = await _unitOfWork.Repository<WorkOrderProcedure>().Entities()
                    .Where(x => x.WorkOrderId.ToString() == request.EntityId)
                    .Select(x => new ProcedurePagingResponse()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    })
                    .ToListAsync();

                var queryResult = await (from c in query
                                         join pM in procedureMapping on c.Id equals pM.ProcedureId into pmg
                                         from pg in pmg.DefaultIfEmpty()
                                         join p in workOrder on pg.EntityId equals p.Id into wog
                                         from wg in wog.DefaultIfEmpty()
                                         select new ProcedurePagingResponse()
                                         {
                                             Id = c.Id,
                                             Name = c.Name,
                                             AssetTypeId = c.AssetTypeId,
                                             AssetType = c.AssetType != null ? c.AssetType.Name : null,
                                             Category = "WorkOrder",
                                             Frequency = null,
                                             FrequencyOccurrence = 0,
                                             Description = c.Description,
                                             Questions = _mapper.Map<List<ProcedureQuestionResponse>>(c.Questions),
                                             Sections = _mapper.Map<List<ProcedureSectionResponse>>(c.Sections),
                                             ProcedureMappings = new List<ProcedureMappingResponse>
                                                 {
                                                     new ProcedureMappingResponse()
                                                     {
                                                         Id = pg != null ? pg.Id : Guid.Empty,
                                                         EntityId = wg != null ? wg.Id : Guid.Empty,
                                                         EntityType = pg != null ? pg.EntityType : null,
                                                         ProcedureId = c.Id,
                                                         EntityCode = wg != null ? wg.WorkOrderId : null,
                                                         Problem = wg != null ? wg.Problem : null,
                                                         TechnicianName = wg != null && wg.Technician != null ? wg.Technician.Name : null,
                                                         Responses = pg != null ? _mapper.Map<List<Responses.Procedures.ProcedureResponse>>(pg.Responses) : new List<Responses.Procedures.ProcedureResponse>()
                                                     }
                                                 }
                                         }).ToListAsync();

                data = queryResult.Concat(workOrderProcedure).ToList();
            }

            if (!String.IsNullOrEmpty(request.StringSearch))
                data = data.Where(x => x.Name.Contains(request.StringSearch, StringComparison.OrdinalIgnoreCase)).ToList();

            var procedures = data.ToPaginatedEnumerableList(request.PageNumber, request.PageSize);
            return Result<PaginatedResult<ProcedurePagingResponse>>.Success(procedures);
        }
        catch (Exception ex)
        {
            return Result<PaginatedResult<ProcedurePagingResponse>>.Fail(ex.Message);
        }
    }
}