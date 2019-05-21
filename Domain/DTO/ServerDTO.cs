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

        // public string PowerState { get; set; }
        // public string VmState { get; set; }

        #endregion

    }
}
