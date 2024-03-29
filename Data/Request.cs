﻿using System.ComponentModel.DataAnnotations;
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
        [Required]
        public bool External { get; set; }

        [ForeignKey("Response")]
        public int? ResponseID { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }

        public string? DocumentKey { get; set; }
        public string? AgreementKey { get; set; }
        public string? ApiInfo { get; set; }
        public Guid? MappedGuid { get; set; }
        public Guid? InquireId { get; set; }


        public bool Validate()
        {
            if (this.Date >= DateTime.UtcNow)
                return false;

            if (this.Amount < 0)
                return false;

            if (NumberOfInstallments <= 0)
                return false;

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

            if (string.IsNullOrWhiteSpace(Status))
                return false;

            if (IncomeLevel <= 0)
                return false;

            return true;
        }
    }
    public class RequestDTO
    {
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
        public string ApiInfo { get; set; }
        public RequestDTO(DateTime date, decimal amount, int numberOfInstallments, string name, string surname, string govermentId, string email, string jobType, decimal incomeLevel, string status)
        {
            Date = date;
            Amount = amount;
            NumberOfInstallments = numberOfInstallments;
            Name = name;
            Surname = surname;
            GovermentId = govermentId;
            Email = email;
            JobType = jobType;
            IncomeLevel = incomeLevel;
            Status = status;
        }
    }
    enum RequestStatus
    {
        Pending,
        OfferSelected,
        DocumentsProvided,
        Approved,
        FinalApproved,
        FinalRefused,
        ExternalClosed
    }

}