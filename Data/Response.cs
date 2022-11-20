using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.Data
{
    public class Response
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResponseID { get; set; }
        
        //public int RequestID { get; set; }
        //public int? UserID { get; set; }
        //public int? UID { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public decimal MonthlyInstallment { get; set; }

        public User? User { get; set; }
        [Required]
        public Request Request { get; set; }
        [Required]
        public string UserEmail { get; set; }

    }
}
