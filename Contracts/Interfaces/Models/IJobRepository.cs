using Entities.Models.Views;

namespace Contracts.interfaces.Models
{
    public interface IJobRepository
    {
        Task<IEnumerable<Job>> GetAllAsync();
    }
}
