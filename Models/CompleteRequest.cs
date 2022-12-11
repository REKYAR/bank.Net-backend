using Bank.NET___backend.Data;

namespace Bank.NET___backend.Models
{
    public class CompleteRequest
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfInstallments { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GovermentId { get; set; }
        public string Email { get; set; }
        public string JobType { get; set; }
        public decimal IncomeLevel { get; set; }
        public string Status { get; set; }

        public CompleteRequest(User user, Request request)
        {
            Date = request.Date;
            Amount = request.Amount;
            NumberOfInstallments = request.NumberOfInstallments;
            Name = request.Name;
        }
    }
}
