using System;
using System.Collections.Generic;
using System.Text;
using SaeedrezayiWebsite.Api.Domain.Common;

namespace SaeedrezayiWebsite.Api.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
    }
}
