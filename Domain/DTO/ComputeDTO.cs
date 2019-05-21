using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class ComputeDTO
    {
        #region Generated Properties        
        [JsonProperty("server")]
        public ServerDTO Server { get; set; }
        [JsonProperty("image")]
        public ImageDTO Image { get; set; }
        [JsonProperty("flavor")]
        public FlavorDTO Flavor { get; set; }

        #endregion

    }
}
