namespace Bank.NET___backend.Models
{
    public class Offer
    {
        public int ID { get; set; }
        public int RID { get; set; }
        public int? UID { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string Status { get; set; }
        
    }
}
