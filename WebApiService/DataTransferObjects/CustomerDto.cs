using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }        
        public bool IsMailing { get; set; }
    }
}
