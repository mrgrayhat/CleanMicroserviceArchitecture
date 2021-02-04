using System;
using System.Collections.Generic;
using System.Text;

namespace BlogModule.Domain.Entities
{
    public class Culture
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
    }
}

