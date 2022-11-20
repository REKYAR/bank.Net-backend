using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.Data
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestID { get; set; }
        //public int? UserID { get; set; }
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
        [ForeignKey("Response")]
        public int ResponseID { get; set; }

        //[ForeignKey("UserID")]
        public User? User { get; set; }
        public Response? Response { get; set; }
    }
}
