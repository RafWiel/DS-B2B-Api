﻿using System.ComponentModel.DataAnnotations;

namespace WebApiService.DataTransferObjects
{
    public class CustomerCompanyListDto
    {        
        [Required]
        public int Id { get; set; }        
        
        [Required]
        [StringLength(64)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(32)]
        public string PhoneNumber { get; set; }        
    }
}
