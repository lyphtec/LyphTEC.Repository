using LyphTEC.Repository.Tests.Domain;

namespace LyphTEC.Repository.Tests
{
    public class CommonRepositoryFixture
    {
        public CommonRepositoryFixture()
        {
            CustomerRepo = new InMemoryRepository<Customer>();
        }

        public IRepository<Customer> CustomerRepo { get; private set; }

        public void ClearRepo()
        {
            CustomerRepo.RemoveAll();
        }
    }
}
