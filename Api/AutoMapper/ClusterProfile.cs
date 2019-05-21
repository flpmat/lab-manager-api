using System;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;

namespace Application.AutoMapper
{
    public partial class ClusterProfile
        : Profile
    {
        public ClusterProfile()
        {
            CreateMap<Domain.Entities.Cluster, ClusterDTO>().ReverseMap();
        }

    }
}
