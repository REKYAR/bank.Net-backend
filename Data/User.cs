using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.Data
{
    public class User
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
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

        //public ICollection<Request> Requests { get; set; }
        //public ICollection<Response> Responses { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;

            if (string.IsNullOrWhiteSpace(Surname))
                return false;

            if (string.IsNullOrWhiteSpace(GovermentId))
                return false;

            if (string.IsNullOrWhiteSpace(Email))
                return false;

            if (string.IsNullOrWhiteSpace(JobType))
                return false;

            if (IncomeLevel < 0)
                return false;

            return true;
        }
    }
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string GovermentId { get; set; }
        public string Email { get; set; }
        public string JobType { get; set; }
        public decimal IncomeLevel { get; set; }
    }

}
