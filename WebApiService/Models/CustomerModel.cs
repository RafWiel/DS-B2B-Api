using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.Models
{
    [Table("Customers")]
    public class CustomerModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? CompanyModelId { get; set; }

        public int? UserId { get; set; }        

        [Required]
        public UserModel User { get; set; }
        
        [Required]
        public byte Type { get; set; }

        [Required]
        public bool IsMailing { get; set; }       
    }
}
