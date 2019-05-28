using System;
using System.Linq;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Controllers
{

    /// <summary>
    /// Sistema Controller without Access Control Layer
    /// </summary>
    //Para excluir as rotas desse controller
    //[ApiExplorerSettings(IgnoreApi = true)] 
    [Route("api/[controller]")]
    public class ClusterController : Controller
    {
        /// <summary>
        /// GetAll Auditoria Operacional
        /// </summary>
        /// <response code="200">list returned all Cluster</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("")]
        public async Task<IActionResult> GetAll([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :

            //GetAll Auditorias Operacionais incluindo relação com Anotação e com Tipo Auditoria Operacional e Tipo Situacao Auditoria Operacional
            var retorno = service.GetAll(
                                    //predicate: p => p.IdSistema ==2,
                                    //orderBy: o => o.OrderBy(o2 => o2.NomeS;istema),
                                    //  include: source => source
                                    // .Include(x => x.TipoSituacaoCluster)
                                    // .Include(x => x.TipoCluster)
                                    // .Include(x => x.AreaAtuacao)
                                    //.ThenInclude(s => s.SegPagina)
                                    ).ToList();
            #endregion

            foreach (var item in retorno)
            {
                item.Server = await service.GetServer(item.IdServer);
            }

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound
            if (!retorno.Any())
                return NotFound();

            return Ok(retorno);
        }

        /// <summary>
        /// GetAll Openstack Images
        /// </summary>
        /// <response code="200">list returned all Openstack Images</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("GetAllImages")]
        public IActionResult GetAllImages([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :
            var retorno = service.GetAllImages();
            #endregion

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound

            return Ok(retorno.Result);
        }


        /// <summary>
        /// GetAll Openstack Flavors
        /// </summary>
        /// <response code="200">list returned all Openstack Flavors</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("GetAllFlavors")]
        public IActionResult GetAllFlavors([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :
            var retorno = service.GetAllFlavors();
            #endregion

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound

            return Ok(retorno.Result);
        }

        /// <summary>
        /// GetAll Openstack Flavors
        /// </summary>
        /// <response code="200">list returned all Openstack Flavors</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("GetAllServers")]
        public IActionResult GetAllServers([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :
            var retorno = service.GetAllServers();
            #endregion

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound

            return Ok(retorno.Result);
        }

        /// <summary>
        /// GetAll Openstack Networks
        /// </summary>
        /// <response code="200">list returned all Openstack Networks</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("GetAllNetworks")]
        public IActionResult GetAllNetworks([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :
            var retorno = service.GetAllNetworks();
            #endregion

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound

            return Ok(retorno.Result);
        }

              /// <summary>
        /// GetAll Openstack Networks
        /// </summary>
        /// <response code="200">list returned all Openstack Networks</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("GetAllNetworks")]
        public IActionResult RebootServer([FromServices] ClusterService service)
        {

            //Exemplos:
            #region : Return using EntityFrameWork (ORM) :
            var retorno = service.GetAllNetworks();
            #endregion

            #region :: Return using DAPPER ::
            //string sql = "select * from Sistema";

            //var retorno = service.Query(sql).ToList();
            #endregion

            //Retornos vazios são NotFound

            return Ok(retorno.Result);
        }


        /// <summary>
        /// Get auditoria operacional By Id
        /// </summary>
        /// <param name="service"></param>
        /// <param name="id"></param>
        /// <response code="200">list returned sistema</response>
        /// <response code="301">moved permanently</response>
        /// <response code="304">not modified</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpGet("{id}")]
        public object GetById([FromServices] ClusterService service, int id)
        {

            var retorno = service.GetAll(predicate: x => x.IdCluster == id
                                            //  include: source => source
                                            // .Include(x => x.TipoSituacaoCluster)
                                            // .Include(x => x.TipoCluster)
                                            // .Include(x => x.AreaAtuacao)
                                            ).FirstOrDefault();

            //Retornos vazios são NotFound
            if (retorno == null)
                return NotFound();

            return Ok(retorno);
        }

        /// <summary>
        /// Cria uma nova auditoria operacional, usando DTO (Data Transfer Object) mapeado com a Entidade.
        /// </summary>
        /// <param name="ClusterDto"></param>
        /// <response code="201">created</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="500">internal error server</response>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromServices] ClusterService service, [FromBody] ClusterDTO ClusterDto)
        {

            try
            {
                var retornoNetwork = await service.CreateNetwork(ClusterDto.NomeCluster);
                var retornoSubnet = await service.CreateSubnet(retornoNetwork.Id, ClusterDto.NomeCluster);
                var retornoRouter = await service.CreateRouter(ClusterDto.NomeCluster);
                var retornoInterface = await service.AddRouterInterface(retornoSubnet.Id, retornoRouter.Id);

                var retorno = await service.CreateServer(ClusterDto.NomeCluster, ClusterDto.IdImage, ClusterDto.IdFlavor, retornoNetwork.Id);

                ClusterDto.IdServer = retorno.Id;
                ClusterDto.IdNetwork = retornoNetwork.Id;

                var interfacePort = await service.GetAllServerInterfaces(retorno.Id);
                
                var floatingIp = await service.AssociateFloatingIp("78c9ed54-cca8-4f1b-837d-b15c185cce17", interfacePort[0].PortId);
                ClusterDto.FloatingIP = floatingIp.FloatingIpAddress;
                ClusterDto.DataCriacao = DateTime.Now;
            }
            catch
            {
                return null;
            }


            var mapped = Mapper.Map<Cluster>(ClusterDto);
            var result = service.Add<ClusterValidator>(mapped);
            return new ObjectResult(result);
        }

        /// <summary>
        /// Atualiza uma auditoria operacional, usando DTO (Data Transfer Object) mapeado com Entidades.
        /// </summary>
        /// <param name="ClusterDto"></param>
        /// <param name="id"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpPut("{id}")]
        public IActionResult Update([FromServices] ClusterService service, [FromBody] ClusterDTO ClusterDto, int id)
        {
            var ClusterRepo = service.GetById(predicate: x => x.IdCluster == id);
            var mapped = Mapper.Map(ClusterDto, ClusterRepo);
            var result = service.Update<ClusterValidator>(mapped);
            //Retornos vazios são NotFound
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Deleta uma auditoria operacional, usando DTO (Data Transfer Object) mapeado com Entidade.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">success</response>
        /// <response code="400">incorrect request</response>
        /// <response code="401">not authorized</response>
        /// <response code="404">resource not found</response>
        /// <response code="500">internal error server</response>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromServices] ClusterService service, int id)
        {
            var ClusterRepo = service.GetById(predicate: x => x.IdCluster == id);

            //Retornos vazios são NotFound
            if (ClusterRepo == null)
                return NotFound();

            var mapped = Mapper.Map<Cluster>(ClusterRepo);

            service.Delete(mapped);

            return new NoContentResult();
        }
    }
}