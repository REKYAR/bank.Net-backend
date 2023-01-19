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

        //public int? UserId { get; set; }
        [Required]
        [ForeignKey("Request")]
        public int RequestID { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public bool External { get; set; }
        public string? ApiInfo { get; set; }
        public Guid? OfferId { get; set; }
        public string? documentKey { get; set; }

        public bool Validate()
        {
            if (string.IsNullOrWhiteSpace(State))
                return false;

            if (MonthlyInstallment < 0)
                return false;

            if (string.IsNullOrWhiteSpace(UserEmail))
                return false;

            return true;
        }
    }
    public class ResponseDTO
    {
        public string State { get; set; }
        public decimal MonthlyInstallment { get; set; }
        public string UserEmail { get; set; }

        public ResponseDTO(string state, decimal monthlyInstallment, string userEmail)
        {
            State = state;
            MonthlyInstallment = monthlyInstallment;
            UserEmail = userEmail;
        }
    }
    enum ResponseStatus
    {
        PendingConfirmation,
        PendingApproval,
        Approved,
        Refused,
        FinalApproved,
        FinalRefused,
        ExternalClosed
    }
}
