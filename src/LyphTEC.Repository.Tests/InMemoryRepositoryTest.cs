using System;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;
using System.Threading.Tasks;
using LyphTEC.Repository.Tests.Domain;
using Xunit;

namespace LyphTEC.Repository.Tests
{
    public class InMemoryRepositoryTest : IClassFixture<CommonRepositoryFixture>
    {
        private readonly CommonRepositoryFixture _fixture;

        public InMemoryRepositoryTest(CommonRepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Repo_Init_UsesSameDatastore()
        {
            _fixture.ClearRepo();

            var cust1 = Generator.Generate<Customer>();
            _fixture.CustomerRepo.Save(cust1);

            var repo2 = new InMemoryRepository<Customer>();

            Assert.Equal(1, repo2.Count());

            var cust2 = repo2.One(1);
            Assert.Equal(cust1, cust2);
        }

        [Fact]
        public void Save_Ok()
        {
            _fixture.ClearRepo();

            var cust = Generator.Generate<Customer>();

            _fixture.CustomerRepo.Save(cust);
            
            Assert.Equal(1, (int)cust.Id);

            var cust2 = Generator.Generate<Customer>();
            cust2.Address = Generator.Generate<Address>();
            _fixture.CustomerRepo.Save(cust2);

            Assert.Equal(2, _fixture.CustomerRepo.Count());
            Assert.Equal(2, (int)cust2.Id);
        }
        
        [Fact]
        public void SaveAll_Ok()
        {
            _fixture.ClearRepo();

            var custs = Generator.Generate<Customer>(3).ToList();

            _fixture.CustomerRepo.SaveAll(custs);

            Assert.Equal(3, _fixture.CustomerRepo.Count());
        }

        [Fact]
        public void Save_Update_Ok()
        {
            _fixture.ClearRepo();

            var cust = Generator.Generate<Customer>();
            _fixture.CustomerRepo.Save(cust);

            cust.Email = "updated";

            var actual = _fixture.CustomerRepo.Save(cust);

            Assert.Equal(actual.Email, cust.Email);
        }

        [Fact]
        public void One_Linq_Ok()
        {
            _fixture.ClearRepo();

            var cust = Generator.Generate<Customer>(x => x.Email = "jsmith@acme.com");
            _fixture.CustomerRepo.Save(cust);

            var actual = _fixture.CustomerRepo.One(x => x.Email.Equals(cust.Email));

            Assert.NotNull(actual);
            Assert.Equal(cust, actual);
        }

        [Fact]
        public void RemoveById_Ok()
        {
            _fixture.ClearRepo();

            _fixture.CustomerRepo.SaveAll([.. Generator.Generate<Customer>(3)]);

            _fixture.CustomerRepo.Remove(2);

            Assert.Equal(2, _fixture.CustomerRepo.Count());

            var cust = _fixture.CustomerRepo.One(2);
            Assert.Null(cust);
        }

        [Fact]
        public void Remove_Ok()
        {
            _fixture.ClearRepo();

            var cust = Generator.Generate<Customer>();
            _fixture.CustomerRepo.Save(cust);

            Assert.Equal(1, _fixture.CustomerRepo.Count());

            _fixture.CustomerRepo.Remove(cust);

            Assert.Equal(0, _fixture.CustomerRepo.Count());
        }

        [Fact]
        public void RemoveByIds_Ok()
        {
            _fixture.ClearRepo();

            _fixture.CustomerRepo.SaveAll([.. Generator.Generate<Customer>(3)]);
            Assert.Equal(3, _fixture.CustomerRepo.Count());

            _fixture.CustomerRepo.RemoveByIds(new[] { 1, 3 });

            Assert.Equal(1, _fixture.CustomerRepo.Count());
        }

        [Fact]
        public void All_Linq_Ok()
        {
            _fixture.ClearRepo();

            _fixture.CustomerRepo.SaveAll([.. Generator.Generate<Customer>(3, x => x.Company = "ACME")]);
            _fixture.CustomerRepo.Save(Generator.Generate<Customer>(x => x.Company = "FooBar"));

            Assert.Equal(4, _fixture.CustomerRepo.Count());

            var actual = _fixture.CustomerRepo.All(x => x.Company.Equals("ACME"));

            Assert.Equal(3, actual.Count());
        }

        // async support in XUnit : http://bradwilson.typepad.com/blog/2012/01/xunit19.html
        [Fact]
        public async Task SaveAsync_Ok()
        {
            _fixture.ClearRepo();

            _fixture.CustomerRepo.SaveAll([.. Generator.Generate<Customer>(3, x => x.Company = "ACME")]);

            var cust = Generator.Generate<Customer>(x => x.Company = "FooBar");

            Console.WriteLine("ThreadId before await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            var actual = await _fixture.CustomerRepoAsync.SaveAsync(cust);

            Console.WriteLine("ThreadId after await call: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

            Assert.Equal(4, (int)actual.Id);
            Assert.Equal(4, _fixture.CustomerRepo.Count());
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

            _fixture.ClearRepo();
            _fixture.CustomerRepo.SaveAll([.. Generator.Generate<Customer>(3, x => x.Company = "ACME")]);

            var cust = Generator.Generate<Customer>(x => x.Company = "MEFFY");

            MefCustomerRepo.Save(cust);

            Assert.Equal(4, (int)cust.Id);
            Assert.Equal(4, _fixture.CustomerRepo.Count());
        }

    }
}
