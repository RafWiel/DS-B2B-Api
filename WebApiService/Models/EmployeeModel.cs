using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.Models
{
    [Table("Employees")]
    public class EmployeeModel : IEntity
    {
        [Key]
        public int Id { get; set; }

        public int? UserId { get; set; }

        public UserModel User { get; set; }

        [Required]
        public int Type { get; set; }              
        
        public bool IsMailing { get; set; }
    }
}
