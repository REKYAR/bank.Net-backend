﻿using System.ComponentModel.DataAnnotations;
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

        public ICollection<Request> Requests { get; set; }
        public ICollection<Response> Responses { get; set; }
    }
}