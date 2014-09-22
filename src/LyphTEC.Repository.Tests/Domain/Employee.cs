using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository.Tests.Domain
{
    public class Employee : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public dynamic ReportsTo { get; set; }   // Points to Employee.Id for hierarchical relationship
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public Address HomeAddress { get; set; }
    }
}
