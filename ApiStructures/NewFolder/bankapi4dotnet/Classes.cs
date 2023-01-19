using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.NET___backend.ApiStructures.NewFolder.bankapi4dotnet
{

    public class Inquiry
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MoneyAmount { get; set; }
        public int InstallmentsCount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DocumentType { get; set; }
        public string DocumentId { get; set; }
        public int JobType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncomeLevel { get; set; }

        public Inquiry()
        {

        }
        public Inquiry(Guid _id, DateTime _creationDate, decimal _moneyAmount, int _installmentsCount, string _firstName, string _lastName,
            int _documentType, string _documentId, int _jobType, decimal _incomeLevel)
        {
            //enforce invariants
            Id = _id;
            CreationDate = _creationDate;
            MoneyAmount = _moneyAmount;
            InstallmentsCount = _installmentsCount;
            FirstName = _firstName;
            LastName = _lastName;
            DocumentType = _documentType;
            DocumentId = _documentId;
            JobType = _jobType;
            IncomeLevel = _incomeLevel;
        }
    }
    public class CreateInquiryRequest
    {
        public decimal MoneyAmount { get; }
        public int InstallmentsNumber { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public int DocumentType { get; }
        public string DocumentId { get; }
        public int JobType { get; }
        public decimal IncomeLevel { get; }

        public CreateInquiryRequest(decimal moneyAmount, int installmentsNumber, string firstName, string lastName, int documentType, string documentId, int jobType, decimal incomeLevel)
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
        public Guid OfferId { get; }
        public CreateInquiryResponse(Guid _inquireId, DateTime _creationDate, Guid _offerId)
        {
            InquireId = _inquireId;
            CreationDate = _creationDate;
            OfferId = _offerId;
        }
    }
    public class CreateInquiry
    {
        public Guid InquireId { get; }
        public DateTime CreationDate{ get; }
        public CreateInquiry(Guid _inquireId, DateTime _creationDate)
        {
            InquireId = _inquireId;
            CreationDate = _creationDate;
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
