using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LyphTEC.Repository.Tests.Domain;
using Xunit;
using ServiceStack.Text;

namespace LyphTEC.Repository.Tests
{
    public class InMemoryRepositoryTest
    {
        private readonly InMemoryRepository<Customer> _repo = new InMemoryRepository<Customer>();
        
        private Customer AddCustomer(string firstName = "John", string lastName = "Smith", string email = "jsmith@acme.com", string company = "ACME")
        {
            var cust = new Customer
                           {
                               FirstName = firstName,
                               LastName = lastName,
                               Company = company,
                               Email = email
                           };

            return _repo.Save(cust);
        }

        private void AddCustomers()
        {
            var custs = new List<Customer>
                            {
                                AddCustomer(),
                                AddCustomer("Jane", "Doe", "jdoe@acme.com"),
                                AddCustomer("Jack", "Wilson", "jwilson@acme.com")
                            };

            _repo.SaveAll(custs);
        }

        [Fact]
        public void Repo_Init_UsesSameDatastore()
        {
            _repo.RemoveAll();

            var cust1 = AddCustomer();

            var repo2 = new InMemoryRepository<Customer>();

            Assert.True(repo2.Count() == 1);

            var cust2 = repo2.One(1);
            Assert.Equal(cust1, cust2);
        }

        [Fact]
        public void Save_Succeeds()
        {
            _repo.RemoveAll();

            var cust = AddCustomer();

            Assert.True(_repo.Count() == 1);
            Assert.True((int)cust.Id == 1);

            var cust2 = AddCustomer();

            Assert.True(_repo.Count() == 2);
            Assert.True((int)cust2.Id == 2);
        }

        [Fact]
        public void SaveAll_Succeeds()
        {
            _repo.RemoveAll();

            AddCustomers();

            Assert.True(_repo.Count() == 3);

            _repo.All().PrintDump();
        }

        [Fact]
        public void Save_Update_Succeeds()
        {
            _repo.RemoveAll();

            var cust = AddCustomer();

            cust.Email = "updated";

            var actual = _repo.Save(cust);

            Assert.True(actual.Email.Equals("updated"));
        }

        [Fact]
        public void One_Linq_Succeeds()
        {
            _repo.RemoveAll();

            var cust = AddCustomer();

            var actual = _repo.One(x => x.Email.Equals("jsmith@acme.com"));

            Assert.NotNull(actual);
            Assert.Equal(cust, actual);
        }

        [Fact]
        public void RemoveById_Succeeds()
        {
            _repo.RemoveAll();

            AddCustomers();

            _repo.Remove(2);

            Assert.True(_repo.Count() == 2);

            var cust = _repo.One(2);
            Assert.Null(cust);
        }

        [Fact]
        public void Remove_Succeeds()
        {
            _repo.RemoveAll();

            var cust = AddCustomer();

            Assert.True(_repo.Count() == 1);
            
            _repo.Remove(cust);

            Assert.True(_repo.Count() == 0);
        }

        [Fact]
        public void RemoveByIds_Succeeds()
        {
            _repo.RemoveAll();

            AddCustomers();
            Assert.True(_repo.Count() == 3);
            
            _repo.RemoveByIds(new[]{1,3});

            Assert.True(_repo.Count() == 1);
            
            _repo.All().PrintDump();
        }

        [Fact]
        public void All_Linq_Succeeds()
        {
            _repo.RemoveAll();

            AddCustomers();

            _repo.Save(AddCustomer("James", "Harrison", "jharrison@foobar.com", "FooBar"));
            Assert.True(_repo.Count() == 4);
            _repo.All().PrintDump();

            var actual = _repo.All(x => x.Company.Equals("ACME"));

            Assert.True(actual.Count() == 3);
            
            Console.WriteLine("After filter:");
            actual.PrintDump();
        }

        // async support in XUnit : http://bradwilson.typepad.com/blog/2012/01/xunit19.html
        [Fact]
        public async Task SaveAync_Succeeds()
        {
            _repo.RemoveAll();

            AddCustomers();

            var cust = new Customer
                           {
                               FirstName = "James",
                               LastName = "Harry",
                               Email = "jharry@foobar.com",
                               Company = "FooBar"
                           };

            Console.WriteLine("ThreadId before await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            var actual = await _repo.SaveAsync(cust);

            Console.WriteLine("ThreadId after await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            Assert.True((int)actual.Id == 4);
            Assert.True(_repo.Count() == 4);
            
            _repo.All().PrintDump();
        }


        [Import]
        public IRepository<Customer> MefCustomerRepo { get; set; }

        [Fact]
        public void MEF_Test()
        {
            var config = new ContainerConfiguration().WithAssembly(typeof (InMemoryRepository<>).Assembly);
            using (var container = config.CreateContainer())
            {
                container.SatisfyImports(this);
            }

            _repo.RemoveAll();
            AddCustomers();

            var cust = new Customer
                           {
                               FirstName = "MEF",
                               LastName = "Head",
                               Email = "mef@acme.com",
                               Company = "MEFFY"
                           };

            MefCustomerRepo.Save(cust);
            
            Assert.True((int)cust.Id == 4);
            Assert.True(_repo.Count() == 4);
            
            _repo.All().PrintDump();
        }

    }
}
