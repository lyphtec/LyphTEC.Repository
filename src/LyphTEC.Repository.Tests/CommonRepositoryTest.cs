using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyphTEC.Repository.Tests.Domain;
using ServiceStack.Text;
using Xunit;

namespace LyphTEC.Repository.Tests
{
    /// <summary>
    /// Abstract class containing common repository tests that can be used by Repository implementations
    /// </summary>
    public abstract class CommonRepositoryTest
    {
        public IRepository<Customer> CustomerRepo { get; protected set; }
        public IRepositoryAsync<Customer> CustomerRepoAsync { get; protected set; }
        
        public Customer NewCustomer(string firstName = "John", string lastName = "Smith", string email = "jsmith@acme.com", string company = "ACME")
        {
            var cust = new Customer
                           {
                               FirstName = firstName,
                               LastName = lastName,
                               Company = company,
                               Email = email
                           };

            return cust;
        }

        public Address NewAddress(string street = "1 Private Road", string city = "Hidden Valley", string state = "VIC", string postCode = "3033", string country = "Australia")
        {
            var address = new Address
                               {
                                   Street = street, 
                                   City = city, 
                                   State = state, 
                                   PostCode = postCode, 
                                   Country = country
                               };

            return address;
        }
        
        public List<Customer> NewCustomers()
        {
            var custs = new List<Customer>
                            {
                                NewCustomer(),
                                NewCustomer("Jane", "Doe", "jdoe@acme.com"),
                                NewCustomer("Jack", "Wilson", "jwilson@acme.com")
                            };

            return custs;
        }

        public void ClearRepo()
        {
            CustomerRepo.RemoveAll();
        }

        public void DumpRepo()
        {
            CustomerRepo.All().PrintDump();
        }
    }
}
