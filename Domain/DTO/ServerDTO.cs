using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class ServerDTO
    {
        #region Generated Properties
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("accessIPv4")]
        public string Ipv4 { get; set; }
        [JsonProperty("created")]
        public DateTime? Created { get; set; }
        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }
        [JsonProperty("flavor")]
        public FlavorDTO Flavor { get; set; }
        [JsonProperty("flavorRef")]
        public string FlavorRef { get; set; }
        [JsonProperty("image")]
        public ImageDTO Image { get; set; }
        [JsonProperty("imageRef")]
        public string ImageRef { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("networks")]
        public NetworkDTO[] Networks { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("OS-EXT-STS:power_state")]
        public int? OsPowerState
        {
            set { PowerState = value; }
        }
        [JsonProperty("power_state")]
        public int? PowerState { get; set; }
        [JsonProperty("progress")]
        public int? Progress { get; set; }

        [JsonProperty("OS-EXT-STS:task_state")]
        public string OsTaskState
        {
            set { TaskState = value; }
        }
        [JsonProperty("task_state")]
        public string TaskState { get; set; }

        #endregion

    }
}
