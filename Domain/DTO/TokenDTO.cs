using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class TokenDTO
    {
        #region Generated Properties
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("issued_at")]
        public DateTime IssuedAt { get; set; }
        [JsonProperty("expires")]
        public DateTime Expires { get; set; }


        [JsonIgnore]
        [JsonProperty("tenant")]
        public object Tenant { get; set; }
        [JsonIgnore]
        [JsonProperty("audit_ids")]
        public string[] Audit_ids { get; set; }


        #endregion

    }
}
