using Contracts.Pagging;

namespace Contracts.Responses
{
    public class ListOfPagedObjectsResponseModel<T> : ParentResponseModel where T : class
    {
        public PagedResult<T>? Objects { get; set; }
    }
}
