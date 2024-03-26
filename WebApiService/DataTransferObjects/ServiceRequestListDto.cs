using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.DataTransferObjects
{    
    public class ServiceRequestListDto : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }        

        [Required]
        public string Name { get; set; }

        public int Ordinal { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Customer { get; set; }

        [Required]
        public string Company { get; set; }
                
        public string? Employee { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string SubmitType { get; set; }

        [Required]
        public string Status { get; set; }               
    }
}
