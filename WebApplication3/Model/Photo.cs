using System;

namespace SampleAPI.Model
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public bool Ismain { get; set; }
        public User User{ get; set; }
        public int UserId { get; set; }

    }
}