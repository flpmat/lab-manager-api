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
        public async Task<NetworkDTO[]> GetAllNetworks()
        {
            var networks = await HttpClientGetComputeApi<NetworkDTO[]>(_configuration, "networks", "networks");

            return networks;
        }

        public async Task<InterfaceDTO[]> GetAllServerInterfaces(string serverId)
        {
            var networks = await HttpClientGetComputeApi<InterfaceDTO[]>(_configuration, "servers/" + serverId + "/os-interface", "interfaceAttachments");

            return networks;
        }

        public async Task<ServerDTO> CreateServer(string name, string imageRef, string flavorRef, string idNetwork)
        {
            var payload = new ComputeDTO
            {
                Server = new ServerDTO
                {
                    ImageRef = imageRef,
                    Name = name,
                    Networks = new NetworkDTO[] {
                        new NetworkDTO {
                            Uuid = idNetwork
                        }
                    },
                    FlavorRef = flavorRef,
                }
            };

            var server = await HttpClientPostComputeApi<ServerDTO>(_configuration, "servers", "server", payload);
            return server;
        }

        public async Task<NetworkDTO> CreateNetwork(string name)
        {
            var payload = new NeutronDTO
            {
                Network = new NetworkDTO
                {
                    Name = "NETWORK_" + name
                }
            };

            var network = await HttpClientPostNetworkApi<NetworkDTO>(_configuration, "networks", "network", payload);
            return network;
        }

        public async Task<SubnetDTO> CreateSubnet(string networkId, string name)
        {
            var payload = new NeutronDTO
            {
                Subnet = new SubnetDTO
                {
                    NetworkId = networkId,
                    Name = "NETWORK_" + name,
                    Cidr = "10.10.10.0/24",
                    IpVersion = 4
                }
            };

            var subnet = await HttpClientPostNetworkApi<SubnetDTO>(_configuration, "subnets", "subnet", payload);
            return subnet;
        }

        public async Task<RouterDTO> CreateRouter(string name)
        {
            var payload = new NeutronDTO
            {
                Router = new RouterDTO
                {
                    Name = "EXTERNAL_ROUTER_" + name
                }
            };

            var router = await HttpClientPostNetworkApi<RouterDTO>(_configuration, "routers", "router", payload);
            return router;
        }

        public async Task<InterfaceDTO> AddRouterInterface(string subnetId, string routerId)
        {
            var payload = new InterfaceDTO
            {
                SubnetId = subnetId

            };

            var result = await HttpClientPutInterfaceNetworkApi<InterfaceDTO>(_configuration, "routers/" + routerId + "/add_router_interface", "id", payload);

            var payloadExternal = new InterfaceDTO
            {
                SubnetId = "5071db6d-4e9b-4d51-bc71-4e741f99db4f"
            };
            await HttpClientPutInterfaceNetworkApi<InterfaceDTO>(_configuration, "routers/" + routerId + "/add_router_interface", "id", payloadExternal);

            return result;
        }

        public async Task<FloatingIpDTO> AssociateFloatingIp(string networkId, string portId)
        {
            var payload = new NeutronDTO
            {
                FloatingIp = new FloatingIpDTO
                {
                    FloatingNetworkId = networkId,
                    PortId = portId
                }

            };

            var result = await HttpClientPostNetworkApi<FloatingIpDTO>(_configuration, "floatingips", "floatingip", payload);
            return result;
        }


    }
}