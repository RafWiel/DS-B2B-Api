using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace WebApiService.DataTransferObjects
{
    public class ServiceRequestDto
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ClosureDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public int Ordinal { get; set; }
        public string? Name => $"ZLS/{Ordinal}/{CreationDate.ToString("MM")}/{CreationDate.ToString("yy")}";
        public string? CompanyName { get; set; }
        //public int? CustomerId { get; set; }        
        //public CustomerModel? Customer { get; set; }
        //public int? EmployeeId { get; set; }        
        //public EmployeeModel? Employee { get; set; }
        //public int? PrefferedEmployeeId { get; set; }
        //public EmployeeModel? PrefferedEmployee { get; set; }
        //public int? PartnerCompanyId { get; set; }        
        //public CompanyModel? PartnerCompany { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string RequestType { get; set; }
        public string SubmitType { get; set; }
        public string? Invoice { get; set; }        
    }
}
