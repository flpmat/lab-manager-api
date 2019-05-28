using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using FluentValidation;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Services._BaseService.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Services._BaseService
{
    public class BaseService<T> : IService<T> where T : class
    {
        private readonly IUnitOfWork _uow;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFileProvider _fileProvider;
        private readonly IConfiguration _configuration;
        private IUnitOfWork uow;

        public BaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public BaseService(IUnitOfWork uow, IHttpClientFactory httpClientFactory)
        {
            _uow = uow;
            _httpClientFactory = httpClientFactory;
        }

        public BaseService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow;
            _configuration = configuration;
        }

        /// <summary>
        /// Adicionar entidade usando fluent valitador
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public T Add<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());
            var repo = _uow.GetRepository<T>();

            repo.Add(obj);

            //Validate
            _uow.SaveChanges();

            return obj;
        }

        /// <summary>
        ///  Atualizar Entidade usando Fluent Validator
        /// </summary>
        /// <param name="obj"></param>
        /// <typeparam name="V"></typeparam>
        /// <returns></returns>
        public T Update<V>(T obj) where V : AbstractValidator<T>
        {
            Validate(obj, Activator.CreateInstance<V>());
            var objectUpdate = _uow.GetRepository<T>().Single(x => x == obj);
            if (objectUpdate != null)
                _uow.GetRepository<T>().Update(obj);

            //Validate
            _uow.SaveChanges();

            return obj;
        }

        /// <summary>
        /// Deletar Entidade por ID sem validação
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentException("O Id não pode ser zero.");

            var repo = _uow.GetRepository<T>();
            repo.Delete(id);

            //Validate
            _uow.SaveChanges();
        }


        /// <summary>
        /// Deletar Entidade por ID sem validação
        /// </summary>
        /// <param name="id"></param>
        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentException("Entidade não encontrada");

            var repo = _uow.GetRepository<T>();
            repo.Delete(entity);

            //Validate
            _uow.SaveChanges();
        }

        /// <summary>
        /// Recuperar todas as informações da entidade
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            int index = 0,
            int size = 0,
            bool disableTracking = true
        )
        {
            //Set repository you will working
            var repo = _uow.GetRepository<T>();
            return repo.GetList(predicate, orderBy, include, index, size, disableTracking).Items.AsEnumerable();
        }

        public T GetById(Expression<Func<T, bool>> predicate)
        {
            //Set repository you will working
            var repo = _uow.GetRepository<T>();
            //return 
            return repo.Select(predicate);
        }

        public IEnumerable<T> Query(string sql)
        {
            //Set repository you will working
            //var repo = _uow.GetRepository<T>();
            var repo = _uow.GetRepositoryDapper<T>();
            //return
            //return repo.Query(sql).AsEnumerable();
            return repo.GetData(sql).AsEnumerable();
        }

        private void Validate(T obj, AbstractValidator<T> validator)
        {
            if (obj == null)
                throw new Exception("Registros não detectados!");

            validator.ValidateAndThrow(obj);
        }

        public async Task<A> HttpClientGetComputeApi<A>(IConfiguration configuration, string endPoint, string property)
        {
            var client = _httpClientFactory.CreateClient("ComputeAPI");

            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var response = await client.GetAsync(client.BaseAddress + endPoint);
            response.EnsureSuccessStatusCode();
            string conteudo = response.Content.ReadAsStringAsync().Result;

            var parsedObject = JObject.Parse(conteudo);
            var popupJson = parsedObject[property].ToString();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            A Result = JsonConvert.DeserializeObject<A>(popupJson, jsonSerializerSettings);

            return Result;
        }

        public async Task<A> HttpClientPostComputeApi<A>(IConfiguration configuration, string endPoint, string propertyReturn, ComputeDTO payload)
        {
            var client = _httpClientFactory.CreateClient("ComputeAPI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentLength = stringPayload.Length;

            var response = await client.PostAsync(client.BaseAddress + endPoint, httpContent);
            response.EnsureSuccessStatusCode();
            string conteudo = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToString();

            var parsedObject = JObject.Parse(conteudo);
            conteudo = parsedObject[propertyReturn].ToString();

            A retorno = JsonConvert.DeserializeObject<A>(conteudo, jsonSerializerSettings);

            return retorno;
        }

        public async Task<A> HttpClientPostActionComputeApi<A>(IConfiguration configuration, string endPoint, string propertyReturn, ActionDTO payload)
        {
            var client = _httpClientFactory.CreateClient("ComputeAPI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentLength = stringPayload.Length;

            var response = await client.PostAsync(client.BaseAddress + endPoint, httpContent);
            response.EnsureSuccessStatusCode();
            string conteudo = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToString();

            var parsedObject = JObject.Parse(conteudo);
            conteudo = parsedObject[propertyReturn].ToString();

            A retorno = JsonConvert.DeserializeObject<A>(conteudo, jsonSerializerSettings);

            return retorno;
        }

        public async Task<A> HttpClientGetNetworkApi<A>(IConfiguration configuration, string endPoint, string property)
        {
            var client = _httpClientFactory.CreateClient("NetworkAPI");

            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var response = await client.GetAsync(client.BaseAddress + endPoint);
            response.EnsureSuccessStatusCode();
            string conteudo = response.Content.ReadAsStringAsync().Result;

            var parsedObject = JObject.Parse(conteudo);
            var popupJson = parsedObject[property].ToString();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            A Result = JsonConvert.DeserializeObject<A>(popupJson, jsonSerializerSettings);

            return Result;
        }

        public async Task<A> HttpClientPostNetworkApi<A>(IConfiguration configuration, string endPoint, string propertyReturn, NeutronDTO payload)
        {
            var client = _httpClientFactory.CreateClient("NetworkAPI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentLength = stringPayload.Length;

            var response = await client.PostAsync(client.BaseAddress + endPoint, httpContent);
            response.EnsureSuccessStatusCode();
            string conteudo = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToString();

            var parsedObject = JObject.Parse(conteudo);
            conteudo = parsedObject[propertyReturn].ToString();

            A retorno = JsonConvert.DeserializeObject<A>(conteudo, jsonSerializerSettings);

            return retorno;
        }

        public async Task<A> HttpClientPutInterfaceNetworkApi<A>(IConfiguration configuration, string endPoint, string propertyReturn, InterfaceDTO payload)
        {
            var client = _httpClientFactory.CreateClient("NetworkAPI");
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Auth-Token", await GetOpenstackToken(configuration));

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            }));



            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentLength = stringPayload.Length;



            var response = await client.PutAsync(client.BaseAddress + endPoint, httpContent);
            response.EnsureSuccessStatusCode();
            string conteudo = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToString();

            A retorno = JsonConvert.DeserializeObject<A>(conteudo, jsonSerializerSettings);

            return retorno;
        }


        public async Task<String> GetOpenstackToken(IConfiguration configuration)
        {
            var clientToken = _httpClientFactory.CreateClient("IdentityAPI");
            var payload = new TokenAuthDTO
            {
                Auth = new AuthDTO
                {
                    PasswordCredentials = new PasswordCredentialsDTO
                    {
                        Username = "admin",
                        Password = "1234"
                    },
                    TenantName = "admin"
                }

            };
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload));
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");
            httpContent.Headers.ContentLength = stringPayload.Length;

            var responseToken = await clientToken.PostAsync(clientToken.BaseAddress + "tokens", httpContent);
            responseToken.EnsureSuccessStatusCode();
            string conteudoToken = JObject.Parse(responseToken.Content.ReadAsStringAsync().Result)["access"].ToString();

            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            AccessDTO access = JsonConvert.DeserializeObject<AccessDTO>(conteudoToken, jsonSerializerSettings);

            return access.Token.Id;
        }
    }
}
