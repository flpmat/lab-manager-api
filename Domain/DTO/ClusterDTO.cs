using System;
using System.Collections.Generic;

namespace Domain.DTO
{
    public partial class ClusterDTO
    {
        #region Generated Properties
        public int IdCluster { get; set; }
        public string IdServer { get; set; }
        public string IdFlavor { get; set; }
        public string IdImage { get; set; }
        public string NomeCluster { get; set; }
        public DateTime DataCriacao { get; set; }
        public string IdNetwork { get; set; }
        public string FloatingIP { get; set; }
        #endregion

    }
}
