using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class EmployeeDto
    {
        public int Id { get; set; }        
        public string Login { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
    }
}
