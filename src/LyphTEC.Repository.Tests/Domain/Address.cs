using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository.Tests.Domain
{
    public class Address : IValueObject
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }

        public DateTime? DateAdded { get; set; }
    }
}
