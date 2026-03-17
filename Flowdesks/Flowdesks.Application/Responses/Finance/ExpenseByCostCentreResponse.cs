namespace Flowdesks.Application.Responses.Finance
{
    public class ExpenseByCostCentreResponse
    {
        public string CostCentre { get; set; }
        public decimal? Budget { get; set; }
        public decimal Expense { get; set; }
    }
}
