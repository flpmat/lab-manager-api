using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class RouterDTO
    {
        #region Generated Properties
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("external_gateway_info")]
        public ExternalGatewayDTO ExternalGatewayInfo { get; set; }
        #endregion

    }
}
