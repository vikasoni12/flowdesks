using Flowdesks.Application.Features.PPMNotes.Query.GetAll;
using Flowdesks.Application.Specifications.Base;
using Flowdesks.Domain.Entities.PPMs;

namespace Flowdesks.Application.Specifications.PPMs
{
    public class PPMNoteFilterSpecification : Specification<PPMNote>
    {
        public PPMNoteFilterSpecification(GetAllPPMNotesQuery request)
        {
            if (request.PPMId != Guid.Empty && request.RaisedDate != null)
            {
                And(p => p.PPMId.Equals(request.PPMId) && p.RaisedDate.Equals(request.RaisedDate));
            }

            if (!string.IsNullOrEmpty(request.StringSearch))
            {
                And(p => p.Text.Contains(request.StringSearch));
            }
        }
    }
}
