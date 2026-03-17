namespace Flowdesks.Application.Responses.WorkOrder
{
    public class CustomerSatisfactionResponse
    {
        public string ResourceType { get; set; }
        public string ResourceName { get; set; }
        public int WorkOrderCount { get; set; }
        public int Happy { get; set; }
        public int Satisfied { get; set; }
        public int Unsatisfied { get; set; }
    }
}
