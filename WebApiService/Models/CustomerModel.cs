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

        public int? UserId { get; set; }

        [Required]
        public UserModel User { get; set; }

        //public int? CompanyId { get; set; }

        //[Required]
        //public CompanyModel Company { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        public bool IsMailing { get; set; }
    }
}
