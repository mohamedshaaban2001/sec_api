using Entities.Models.Views;

namespace Contracts.interfaces.Models
{
    public interface IPersonsRepository
    {
        Task<IEnumerable<Persons>> GetAllAsync();
    }
}
