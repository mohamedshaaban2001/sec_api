using Contracts.interfaces.Models;
using Entities.Models;
using Entities.Models.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class JobRepository : IJobRepository
    {
        private readonly RepositoryContext _repositoryContext;

        public JobRepository(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _repositoryContext.Jobs.AsNoTracking().ToListAsync();
        }
    }
}
