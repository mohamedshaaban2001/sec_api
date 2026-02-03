using Contracts.interfaces.Models;
using Entities.Models;
using Entities.Models.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Repositories.Models
{
    public class PersonsRepository : IPersonsRepository
    {
        private readonly RepositoryContext _repositoryContext;

        public PersonsRepository(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task<IEnumerable<Persons>> GetAllAsync()
        {
            return await _repositoryContext.Persons.AsNoTracking().ToListAsync();
        }
    }
}
