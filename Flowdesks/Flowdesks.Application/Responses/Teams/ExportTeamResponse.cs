using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowdesks.Application.Responses.Teams;

public class ExportTeamResponse
{

    [Description("Name")]
    public string Name { get; set; }
    [Description("About")]
    public string About { get; set; }
    [Description("Total Member")]
    public string TotalMember { get; set; }

}
