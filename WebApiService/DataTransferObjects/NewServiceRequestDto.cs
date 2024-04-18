using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class NewServiceRequestDto
    {        
        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(64)]
        public string Topic { get; set; }
        
        [StringLength(1024)]
        public string? Description { get; set; }        

        [Required]
        public byte RequestType { get; set; }

        [Required]
        public byte SubmitType { get; set; }
    }
}
