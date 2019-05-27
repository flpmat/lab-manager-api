using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class SubnetDTO
    {
        #region Generated Properties
        [JsonProperty("enable_dhcp")]
        public bool EnableDhcp { get; set; }
        [JsonProperty("network_id")]
        public string NetworkId { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("gateway_ip")]
        public string GatewayIp { get; set; }
        [JsonProperty("cidr")]
        public string Cidr { get; set; }
        [JsonProperty("ip_version")]
        public int IpVersion { get; set; }
        #endregion

    }
}
