using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Entities;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Services._BaseService;

namespace Services
{
    public class ClusterService : BaseService<Cluster>
    {
        private IUnitOfWork _uow;
        private readonly IHttpClientFactory _httpClientFactory;
        protected readonly IConfiguration _configuration;

        public ClusterService(IUnitOfWork uow, IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(uow, httpClientFactory)
        {
            _uow = uow;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<ImageDTO[]> GetAllImages()
        {
            var images = await HttpClientGetComputeApi<ImageDTO[]>(_configuration, "images", "images");

            return images;
        }
        public async Task<FlavorDTO[]> GetAllFlavors()
        {
            var flavors = await HttpClientGetComputeApi<FlavorDTO[]>(_configuration, "flavors/detail", "flavors");

            return flavors;
        }
        public async Task<ServerDTO[]> GetAllServers()
        {
            var servers = await HttpClientGetComputeApi<ServerDTO[]>(_configuration, "servers/detail", "servers");

            return servers;
        }

        public async Task<ServerDTO> CreateServer()
        {
            var payload = new ComputeDTO
            {
                Server = new ServerDTO
                {
                    ImageRef = "363b4b15-4f57-4c82-915d-ef9302d00c12",
                    Name = "LabManagerInstance",
                    Networks = new NetworkDTO[] {
                        new NetworkDTO {
                            Uuid = "78c9ed54-cca8-4f1b-837d-b15c185cce17"
                        },
                        new NetworkDTO {
                            Uuid = "dfb8ebe1-35e0-49f2-b533-75c842a2cea0"
                        }
                    },
                    FlavorRef = "7",
                }
            };
            var server = await HttpClientPostComputeApi<ServerDTO>(_configuration, "servers", "server", payload);
            return server;
        }

    }
}