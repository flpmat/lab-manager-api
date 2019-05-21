using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class AuthDTO
    {
        #region Generated Properties
                [JsonProperty("passwordCredentials")]
        public PasswordCredentialsDTO PasswordCredentials  { get; set; }
        [JsonProperty("tenantName")]
        public string TenantName { get; set; }


        #endregion

    }
}
