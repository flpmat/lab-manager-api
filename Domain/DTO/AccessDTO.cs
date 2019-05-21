using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class AccessDTO
    {
        #region Generated Properties
        [JsonProperty("token")]
        public TokenDTO Token { get; set; }

        #endregion

    }
}
