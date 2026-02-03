using System.Text.Json.Serialization;

namespace Entities.Models.BaseTables
{
    public class BaseTable
    {

      
        public int Id { get; set; }
        [JsonIgnore]
        public string? InsertUserCode { get; set; }
        [JsonIgnore]
        public DateTime? InsertDate { get; set; }
        [JsonIgnore]
        public string? UpdateUserCode { get; set; }
        [JsonIgnore]
        public DateTime? LastUpdate { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        [JsonIgnore]
        public string? DeleteUserCode { get; set; }
        [JsonIgnore]
        public DateTime? DeleteDate { get; set; }
    }
}
