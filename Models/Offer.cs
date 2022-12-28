using Bank.NET___backend.Data;

namespace Bank.NET___backend.Models
{
    public class Offer
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
        public decimal MonthlyInstallment { get; set; }
        public Offer()
        {}
        public Offer(Request r, decimal monthlyInstallment)
        {
            
            Date = DateTime.Now;
            Amount = r.Amount;
            NumberOfInstallments = r.NumberOfInstallments;
            Name = r.Name;
            Surname = r.Surname;
            GovermentId = r.GovermentId;
            Email = r.Email;
            JobType = r.JobType;
            IncomeLevel = r.IncomeLevel;
            Status = r.Status;
            MonthlyInstallment = monthlyInstallment;
        }

        public Offer(RequestDTO r, decimal monthlyInstallment)
        {
            
            Date = DateTime.Now;
            Amount = r.Amount;
            NumberOfInstallments = r.NumberOfInstallments;
            Name = r.Name;
            Surname = r.Surname;
            GovermentId = r.GovermentId;
            Email = r.Email;
            JobType = r.JobType;
            IncomeLevel = r.IncomeLevel;
            Status = r.Status;
            MonthlyInstallment = monthlyInstallment;
        }
        public bool validate(Request r, decimal monthlyInstallment)
        {
            return (Amount == r.Amount &&
                NumberOfInstallments == r.NumberOfInstallments &&
                Name == r.Name &&
                Surname == r.Surname &&
                GovermentId == r.GovermentId &&
                Email == r.Email &&
                JobType == r.JobType &&
                IncomeLevel == r.IncomeLevel &&
                Status == r.Status &&
                MonthlyInstallment == monthlyInstallment
                );
        }
    }
}
