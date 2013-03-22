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
    public class InMemoryRepositoryTest : CommonRepositoryTest, IUseFixture<CustomerRepositoryFixture>
    {
        [Fact]
        public void Repo_Init_UsesSameDatastore()
        {
            ClearRepo();

            var cust1 = NewCustomer();
            CustomerRepo.Save(cust1);

            var repo2 = new InMemoryRepository<Customer>();

            Assert.True(repo2.Count() == 1);

            var cust2 = repo2.One(1);
            Assert.Equal(cust1, cust2);

            DumpRepo();
        }

        [Fact]
        public void Save_Ok()
        {
            ClearRepo();

            var cust = NewCustomer();

            CustomerRepo.Save(cust);
            
            Assert.True((int)cust.Id == 1);

            var cust2 = NewCustomer("Jack", "Wilson", "jwilson@acme.com");
            cust2.Address = NewAddress();
            CustomerRepo.Save(cust2);

            Assert.True(CustomerRepo.Count() == 2);
            Assert.True((int)cust2.Id == 2);

            DumpRepo();
        }
        
        [Fact]
        public void SaveAll_Ok()
        {
            ClearRepo();

            var custs = NewCustomers();

            CustomerRepo.SaveAll(custs);

            Assert.True(CustomerRepo.Count() == 3);

            DumpRepo();
        }

        [Fact]
        public void Save_Update_Ok()
        {
            ClearRepo();

            var cust = NewCustomer();
            CustomerRepo.Save(cust);

            Console.WriteLine("Before update:");
            cust.PrintDump();

            cust.Email = "updated";

            var actual = CustomerRepo.Save(cust);

            Assert.True(actual.Email.Equals("updated"));

            Console.WriteLine("After update");
            actual.PrintDump();
        }

        [Fact]
        public void One_Linq_Ok()
        {
            ClearRepo();

            var cust = NewCustomer();
            CustomerRepo.Save(cust);

            var actual = CustomerRepo.One(x => x.Email.Equals("jsmith@acme.com"));

            Assert.NotNull(actual);
            Assert.Equal(cust, actual);

            actual.PrintDump();
        }

        [Fact]
        public void RemoveById_Ok()
        {
            ClearRepo();

            CustomerRepo.SaveAll(NewCustomers());

            CustomerRepo.Remove(2);

            Assert.True(CustomerRepo.Count() == 2);

            var cust = CustomerRepo.One(2);
            Assert.Null(cust);

            DumpRepo();
        }

        [Fact]
        public void Remove_Ok()
        {
            ClearRepo();

            var cust = NewCustomer();
            CustomerRepo.Save(cust);

            Assert.True(CustomerRepo.Count() == 1);

            CustomerRepo.Remove(cust);

            Assert.True(CustomerRepo.Count() == 0);
        }

        [Fact]
        public void RemoveByIds_Ok()
        {
            ClearRepo();

            CustomerRepo.SaveAll(NewCustomers());
            Assert.True(CustomerRepo.Count() == 3);

            CustomerRepo.RemoveByIds(new[] { 1, 3 });

            Assert.True(CustomerRepo.Count() == 1);

            DumpRepo();
        }

        [Fact]
        public void All_Linq_Ok()
        {
            ClearRepo();

            CustomerRepo.SaveAll(NewCustomers());
            CustomerRepo.Save(NewCustomer("James", "Harrison", "jharrison@foobar.com", "FooBar"));

            Assert.True(CustomerRepo.Count() == 4);
            
            DumpRepo();

            var actual = CustomerRepo.All(x => x.Company.Equals("ACME"));

            Assert.True(actual.Count() == 3);

            Console.WriteLine("After filter:");
            actual.PrintDump();
        }

        // async support in XUnit : http://bradwilson.typepad.com/blog/2012/01/xunit19.html
        [Fact]
        public async Task SaveAsync_Ok()
        {
            ClearRepo();

            CustomerRepo.SaveAll(NewCustomers());

            var cust = new Customer
                           {
                               FirstName = "James",
                               LastName = "Harry",
                               Email = "jharry@foobar.com",
                               Company = "FooBar"
                           };

            Console.WriteLine("ThreadId before await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            var actual = await CustomerRepoAsync.SaveAsync(cust);

            Console.WriteLine("ThreadId after await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            Assert.True((int)actual.Id == 4);
            Assert.True(CustomerRepo.Count() == 4);

            DumpRepo();
        }


        [Import]
        public IRepository<Customer> MefCustomerRepo { get; set; }

        [Fact]
        public void MEF_Test()
        {
            var config = new ContainerConfiguration().WithAssembly(typeof(InMemoryRepository<>).Assembly);
            using (var container = config.CreateContainer())
            {
                container.SatisfyImports(this);
            }

            ClearRepo();
            CustomerRepo.SaveAll(NewCustomers());

            var cust = new Customer
                           {
                               FirstName = "MEF",
                               LastName = "Head",
                               Email = "mef@acme.com",
                               Company = "MEFFY"
                           };

            MefCustomerRepo.Save(cust);

            Assert.True((int)cust.Id == 4);
            Assert.True(CustomerRepo.Count() == 4);

            DumpRepo();
        }


        #region IUseFixture<CustomerRepositoryFixture> Members

        public void SetFixture(CustomerRepositoryFixture data)
        {
            var repo = new InMemoryRepository<Customer>();

            data.Init(repo, repo);

            CustomerRepo = data.CustomerRepo;
            CustomerRepoAsync = data.CustomerRepoAsync;
        }

        #endregion
    }
}
