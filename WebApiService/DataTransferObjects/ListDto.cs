using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApiService.Models;

namespace WebApiService.DataTransferObjects
{
    public class ListDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }        
    }
}
