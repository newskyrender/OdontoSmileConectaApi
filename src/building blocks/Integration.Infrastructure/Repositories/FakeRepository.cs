using Integration.Domain.Entities;
using Integration.Domain.Repositories;
using Integration.Infrastructure.Contexts;
using Integration.Infrastructure.Repositories.Base;

namespace Integration.Infrastructure.Repositories
{
    public class FakeRepository : GenericRepository<Fake>, IFakeRepository
    {
        public FakeRepository(IntegrationDataContext context) : base(context)
        {

        }
    }
}

