using System;
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
    }
}
