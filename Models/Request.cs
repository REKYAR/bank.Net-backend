namespace Bank.NET___backend.Models
{
    public class Request
    {
        public Guid ID { get; set; }
        public Guid? UID { get; set; }
        public DateTime Date { get; set; }
        public Decimal Amount   {get; set; }
        public string GovId { get; set; }
        public int Installments { get; set; }

    }
}
