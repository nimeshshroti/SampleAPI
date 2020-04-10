﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        [StringLength(10, MinimumLength =4,ErrorMessage ="Password should be minimum of 4 characters")]
        public string password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }        
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO () {
            Created = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}
