using Flowdesks.Shared.Wrapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Requests.BulkUpload;
public class BulkUploadRequest : IRequest<Result<int>>
{
    public IFormFile? file {  get; set; }
}
