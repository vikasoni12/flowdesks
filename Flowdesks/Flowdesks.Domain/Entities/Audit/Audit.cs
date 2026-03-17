using Flowdesks.Domain.Common;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;

namespace Flowdesks.Domain.Entities.Audit;

public class Audit : IEntity<int>
{
    public int Id { get; set; }
    public Guid TenantId { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }

    [NotMapped]
    public string PkId
    {
        get { return PrimaryKey == null ? null : JsonConvert.DeserializeObject<ExpandoObject>(PrimaryKey).ToList()[0].Value.ToString(); }
        set { PrimaryKey = JsonConvert.SerializeObject(value); }
    }
}