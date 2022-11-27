using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.Data
{
    public class Request
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RequestID { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public int NumberOfInstallments { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string GovermentId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string JobType { get; set; }
        [Required]
        public decimal IncomeLevel { get; set; }
        [Required]
        public string Status { get; set; }

        [ForeignKey("Response")]
        public int ResponseID { get; set; }

        public User? User { get; set; }
        public Response? Response { get; set; }
    }
}
