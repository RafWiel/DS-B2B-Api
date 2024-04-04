using WebApiService.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiService.DataTransferObjects
{    
    public class ServiceRequestListDto : IEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }        
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }                
        public string Employee { get; set; }
        public string Type { get; set; }
        public string SubmitType { get; set; }
        public string Status { get; set; }        
        public int Ordinal { get; set; }
        public byte TypeNum { get; set; }        
        public byte SubmitTypeNum { get; set; }
        public byte StatusNum { get; set; }
    }
}
