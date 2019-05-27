using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class NeutronDTO
    {
        #region Generated Properties        
        [JsonProperty("network")]
        public NetworkDTO Network { get; set; }
        [JsonProperty("subnet")]
        public SubnetDTO Subnet { get; set; }
        [JsonProperty("router")]
        public RouterDTO Router { get; set; }
        [JsonProperty("interface")]
        public InterfaceDTO Interface { get; set; }
        [JsonProperty("floatingip")]
        public FloatingIpDTO FloatingIp { get; set; }


        #endregion

    }
}
