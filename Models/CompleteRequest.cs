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

        public decimal? Rate { get; set; }

        public int? ResponseID { get; set; }

        public string? ResponseStatus { get; set; }

        public CompleteRequest( Request request, decimal? rate)
        {
            Date = request.Date;
            Amount = request.Amount;
            NumberOfInstallments = request.NumberOfInstallments;
            Name = request.Name;
            Surname = request.Surname;
            GovermentId = request.GovermentId;
            Email = request.Email;
            JobType = request.JobType;
            IncomeLevel = request.IncomeLevel;
            Status = request.Status;
            Rate = rate;
        }

        public CompleteRequest(Response res, Request req)
        {
            Date = req.Date;
            Amount = req.Amount;
            NumberOfInstallments = req.NumberOfInstallments;
            Name = req.Name;
            Surname = req.Surname;
            GovermentId = req.GovermentId;
            Email = req.Email;
            JobType = req.JobType;
            IncomeLevel = req.IncomeLevel;
            Status = req.Status;

            Rate = res.MonthlyInstallment;
            ResponseID = res.ResponseID;
            ResponseStatus = res.State;
        }
    }
}
