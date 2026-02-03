using Entities.Models.Views;

namespace Contracts.interfaces.Models
{
    public interface IUniversityRepository
    {
        Task<IEnumerable<University>> GetAllAsync();
    }
}
