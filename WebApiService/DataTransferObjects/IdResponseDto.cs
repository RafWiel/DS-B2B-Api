using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class IdResponseDto
    {
        [Required]
        public int Id { get; set; }
    }
}
