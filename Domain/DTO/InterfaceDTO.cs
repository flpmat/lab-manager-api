using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class InterfaceDTO
    {
        #region Generated Properties
        [JsonProperty("subnet_id")]
        public string SubnetId { get; set; }
        [JsonProperty("port_id")]
        public string PortId { get; set; }

        #endregion

    }
}
