using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class TokenAuthDTO
    {
        #region Generated Properties
                [JsonProperty("auth")]
        public AuthDTO Auth  { get; set; }


        #endregion

    }
}
