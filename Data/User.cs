namespace Bank.NET___backend.Data
{
    public class User
    {        
        public int ID { get; set; }
        public string PasswordHash { get; set; }//not sure if necessearry
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GovermentId { get; set; }
        public string Email { get; set; }
        public string JobType { get; set; }
        public decimal IncomeLevel { get; set; }
    }
}
