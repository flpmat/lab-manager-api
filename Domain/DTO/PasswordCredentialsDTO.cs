using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class PasswordCredentialsDTO
    {
        #region Generated Properties
        [JsonProperty("username")]
        public string Username { get; set; } 
        [JsonProperty("password")]
        public string Password { get; set; } 
        #endregion

    }
}
