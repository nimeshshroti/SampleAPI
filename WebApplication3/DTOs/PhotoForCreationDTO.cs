﻿using Microsoft.AspNetCore.Http;
using System;

namespace SampleAPI.Controllers
{
    public class PhotoForCreationDTO
    {
        public PhotoForCreationDTO()
        {           
            DateAdded = DateTime.Now;            
        }

        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
    }
}