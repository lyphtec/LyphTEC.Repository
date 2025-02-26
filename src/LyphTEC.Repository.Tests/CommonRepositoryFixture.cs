using LyphTEC.Repository.Tests.Domain;

namespace LyphTEC.Repository.Tests
{
    public class CommonRepositoryFixture
    {
        public CommonRepositoryFixture()
        {
            var repo = new InMemoryRepository<Customer>();

            CustomerRepo = repo;
            CustomerRepoAsync = repo;
        }

        public IRepository<Customer> CustomerRepo { get; private set; }
        public IRepositoryAsync<Customer> CustomerRepoAsync { get; private set; }

        public void ClearRepo()
        {
            CustomerRepo.RemoveAll();
        }
    }
}
