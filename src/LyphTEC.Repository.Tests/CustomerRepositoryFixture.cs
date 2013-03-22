using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyphTEC.Repository.Tests.Domain;

namespace LyphTEC.Repository.Tests
{
    public class CustomerRepositoryFixture
    {
        public void Init(IRepository<Customer> customerRepo, IRepositoryAsync<Customer> customerRepoAsync)
        {
            CustomerRepo = customerRepo;
            CustomerRepoAsync = customerRepoAsync;
        }

        public IRepository<Customer> CustomerRepo { get; protected set; }
        public IRepositoryAsync<Customer> CustomerRepoAsync { get; protected set; }
    }
}
