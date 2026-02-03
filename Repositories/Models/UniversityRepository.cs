using Contracts.interfaces.Models;
using Entities.Models;
using Entities.Models.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class UniversityRepository : IUniversityRepository
    {
        private readonly RepositoryContext _repositoryContext;

        public UniversityRepository(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<University>> GetAllAsync()
        {
            return await _repositoryContext.Universities.AsNoTracking().ToListAsync();
        }
    }
}
