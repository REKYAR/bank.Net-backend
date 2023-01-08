using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.ApiStructures.NewFolder.bankapi4dotnet
{
    public class CreateInquiryRequest
    {
        public decimal MoneyAmount { get; }
        public int InstallmentsNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int DocumentType { get; }
        public int DocumentId { get; }
        public int JobType { get; }
        public decimal IncomeLevel { get; }

        public CreateInquiryRequest(decimal moneyAmount, int installmentsNumber, string firstName, string lastName, int documentType, int documentId, int jobType, decimal incomeLevel)
        {
            MoneyAmount = moneyAmount;
            InstallmentsNumber = installmentsNumber;
            FirstName = firstName;
            LastName = lastName;
            DocumentType = documentType;
            DocumentId = documentId;
            JobType = jobType;
            IncomeLevel = incomeLevel;
        }
    }
    public class CreateInquiryResponse
    {
        public Guid InquireId { get; }
        public DateTime CreationDate{ get; }
        public string StatusDescription { get; }
        public Guid OfferId { get; }
        public CreateInquiryResponse(Guid _inquireId, DateTime _creationDate, string _statusDescription, Guid _offerId)
        {
            InquireId = _inquireId;
            CreationDate = _creationDate;
            StatusDescription = _statusDescription;
            OfferId = _offerId;
        }
    }
    public class FileModel
    {
        public IFormFile ImageFile { get; set; }
        public FileModel(IFormFile file)
        {
            ImageFile = file;
        }
    }
    public class Offer
    {
        public Guid Id { get; set; }
        public int Percentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MonthlyInstallment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal RequesedValue { get; set; }
        public int RequestedPeriodInMonth { get; set; }
        public int StatusID { get; set; }
        public string StatusDescription { get; set; }
        public Guid InquireId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string DocumentLink { get; set; }
        public DateTime DocumentLinkValidDate { get; set; }

        public Offer()
        {
        }

        public Offer(Guid id, int percentage, decimal monthlyInstallment, decimal requesedValue, int requestedPeriodInMonth, int statusID,
            string statusDescription, Guid inquireId, DateTime createdDate, DateTime updatedDate, string documentLink, DateTime documentLinkValidDate)
        {
            Id = id;
            Percentage = percentage;
            MonthlyInstallment = monthlyInstallment;
            RequesedValue = requesedValue;
            RequestedPeriodInMonth = requestedPeriodInMonth;
            StatusID = statusID;
            StatusDescription = statusDescription;
            InquireId = inquireId;
            CreatedDate = createdDate;
            UpdatedDate = updatedDate;
            DocumentLink = documentLink; //edit
            DocumentLinkValidDate = documentLinkValidDate; //edit
        }
    }
}
