using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class ExternalGatewayDTO
    {
        #region Generated Properties
        [JsonProperty("network_id")]
        public string NetworkId { get; set; }
        [JsonProperty("enable_snat")]
        public bool EnableSnat { get; set; }

        #endregion

    }
}
