using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.DTOs
{
    public class MessageForCreationDTO
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreationDTO()
        {
            MessageSent = DateTime.Now;
        }
    }
}
