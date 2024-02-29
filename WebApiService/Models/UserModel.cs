using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.Models
{
    [Table("Users")]
    public class UserModel : IEntity
    {
        [Key]
        public int Id { get; set; }        
       
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
        
        public bool IsActive { get; set; }       
    }
}
