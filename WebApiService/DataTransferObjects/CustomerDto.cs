using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class CustomerDto
    {        
        [Required]
        public int Id { get; set; }

        public int? CompanyId { get; set; }        

        [Required]
        [StringLength(64)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(32)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(64)]
        public string Email { get; set; }

        [Required]
        public byte Type { get; set; }

        [Required]
        public bool IsMailing { get; set; }        
    }
}
