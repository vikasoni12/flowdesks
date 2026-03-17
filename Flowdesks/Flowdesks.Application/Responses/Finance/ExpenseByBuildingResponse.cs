namespace Flowdesks.Application.Responses.Finance
{
    public class ExpenseByBuildingResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Expense { get; set; }
    }
    public class ExpenseBySiteResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TotalExpense { get; set; }
        public List<ExpenseByBuildingResponse> Buildings { get; set; }
    }

}
