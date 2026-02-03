using Contracts.enums;
using System.Text.Json;

namespace Contracts.Responses
{
    public class ParentResponseModel
    {
        /// <summary>
        /// Contain Error Code which defined in error catalog
        /// </summary>
        public ErrorCatalog ErrorCode { get; set; }
        /// <summary>
        /// indicate that the process is successeded or failed
        /// true : success
        /// fail : false
        /// </summary>
        public bool IsDone { get; set; }

        /// <summary>
        /// message describes the operation done or the error that occured
        /// </summary>
        public string? ReturnMessage { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
