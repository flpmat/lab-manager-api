using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class FloatingIpDTO
    {
        #region Generated Properties
        [JsonProperty("floating_network_id")]
        public string FloatingNetworkId { get; set; }
        [JsonProperty("port_id")]
        public string PortId { get; set; }
         [JsonProperty("floating_ip_address")]
        public string FloatingIpAddress { get; set; }
        #endregion

    }
}
