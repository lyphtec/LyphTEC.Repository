using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository.Tests.Domain
{
    public class Customer : Entity, IAggregateRoot
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public Address Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public object SupportRepId { get; set; }    // Ref to Employee.Id
    }
}
