using System;
using System.Collections.Generic;
using System.Text;

namespace STS.Application.Features.Notifications.Models
{
    public class EmailAttachement
    {
        public string Name { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }
    }
}
