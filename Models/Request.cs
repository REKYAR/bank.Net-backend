namespace Bank.NET___backend.Models
{
    public class Request
    {
        public int ID { get; set; }
        public int? UID { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount   {get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string JobType { get; set; }
        public string GovId { get; set; }
        public string Email { get; set; }
        public int Installments { get; set; }
        public int NumberOfInstallments { get; set; }
        public string Status { get; set; }
    }
}
