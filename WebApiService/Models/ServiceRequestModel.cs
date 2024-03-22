using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.Models
{
    [Table("ServiceRequests")]
    public class ServiceRequestModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        public DateTime? ClosureDate { get; set; }

        public DateTime? ReminderDate { get; set; }

        [Required]
        public int Ordinal { get; set; }

        //[NotMapped]
        //public string? Name => $"ZLS/{Ordinal}/{CreationDate.ToString("MM")}/{CreationDate.ToString("yy")}";

        public int? CustomerId { get; set; }
        
        public CustomerModel? Customer { get; set; }

        public int? EmployeeId { get; set; }
        
        public EmployeeModel? Employee { get; set; }

        public int? PrefferedEmployeeId { get; set; }

        public EmployeeModel? PrefferedEmployee { get; set; }

        public int? PartnerCompanyId { get; set; }
        
        public CompanyModel? PartnerCompany { get; set; }

        [Required]
        [StringLength(64)]
        public string Topic { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        [Required]
        public byte Status { get; set; }

        [Required]
        public byte RequestType { get; set; }

        [Required]
        public byte SubmitType { get; set; }

        [StringLength(16)]
        public string? Invoice { get; set; }        
    }
}
