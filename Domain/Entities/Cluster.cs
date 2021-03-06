using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.DTO;

namespace Domain.Entities
{
    [Table("cluster")]
    public partial class Cluster
    {
        public Cluster()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_cluster")]
        public int IdCluster { get; set; }
        [Column("id_server")]
        public string IdServer { get; set; }
        [Column("id_image")]
        public string IdImage { get; set; }
        [Column("id_flavor")]
        public string IdFlavor { get; set; }
        [Column("id_network")]
        public string IdNetwork { get; set; }
        [Column("floating_ip")]
        public string FloatingIP { get; set; }
        // [Column("ipv4")]
        // public string Ipv4 { get; set; }
        [Column("nome_cluster")]
        public string NomeCluster { get; set; }
        [Column("data_criacao")]
        public DateTime DataCriacao { get; set; }

        #endregion

        [NotMapped]
        public ServerDTO Server { get; set; }

        #region Generated Relationships

        #endregion

    }
}
