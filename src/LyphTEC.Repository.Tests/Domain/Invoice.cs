using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LyphTEC.Repository.Tests.Domain
{
    public class Invoice : Entity
    {
        public object CustomerId { get; set; }  // Ref to Customer.Id
        public DateTime InvoiceDate { get; set; }
        public Address BillingAddress { get; set; }
        public decimal Total { get; set; }
    }
}
