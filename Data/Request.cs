namespace Bank.NET___backend.Data
{
    public class Request
    {
        public int ID { get; set; }
        public int? UID { get; set; }
        public decimal Amount { get; set; }
        public int NumberOfInstallments { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GovermentId { get; set; }
        public string Email { get; set; }
        public string JobType { get; set; }
        public decimal IncomeLevel { get; set; }
        public string State { get; set; }
    }
}
