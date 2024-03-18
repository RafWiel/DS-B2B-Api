using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class CustomerListDto
    {
        [Required]
        public int Id { get; set; }        

        [Required]
        [StringLength(64)]
        public string Login { get; set; }
        
        [Required]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [StringLength(32)]
        public string PhoneNumber { get; set; }        

        [Required]
        [StringLength(64)]
        public string CompanyName { get; set; }

        [Required]
        public byte Type { get; set; }
    }
}
