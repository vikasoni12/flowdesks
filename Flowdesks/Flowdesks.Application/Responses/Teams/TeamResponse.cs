namespace Flowdesks.Application.Responses.Teams
{
    public class TeamResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        public int TotalMember {  get; set; }
        public List<TeamUserResponse> TeamUsers { get; set; }
        public List<TeamSiteResponse> TeamSites { get; set; }
        public List<TeamBuildingResponse> TeamBuildings { get; set; }
    }
}
