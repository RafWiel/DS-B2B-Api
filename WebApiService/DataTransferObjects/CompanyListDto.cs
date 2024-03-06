using System.ComponentModel.DataAnnotations;
using WebApiService.Models;

namespace WebApiService.DataTransferObjects
{
    public class CompanyListDto
    {        
        [Required]
        public int Id { get; set; }

        [Required]
        public int ErpId { get; set; }
      
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(16)]
        public string TaxNumber { get; set; }
                

        [Required]
        [StringLength(16)]
        public string City { get; set; }        
    }
}
