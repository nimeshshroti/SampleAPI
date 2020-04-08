using Microsoft.AspNetCore.Http;
using System;

namespace SampleAPI.Controllers
{
    public class PhotoForReturnDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public bool isMain { get; set; }
        public string PublicId { get; set; }
    }
}