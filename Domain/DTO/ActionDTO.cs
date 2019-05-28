using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Domain.DTO
{
    public partial class ActionDTO
    {
        #region Generated Properties        
        [JsonProperty("os-start")]
        public int? OsStart { get; set; }
        [JsonProperty("os-stop")]
        public int? OsStop { get; set; }
        [JsonProperty("reboot")]
        public int? Reboot { get; set; }
        [JsonProperty("pause")]
        public int? Pause { get; set; }
        [JsonProperty("unpause")]
        public int? Unpause { get; set; }

        #endregion

    }
}
