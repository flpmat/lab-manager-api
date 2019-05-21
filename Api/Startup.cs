using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using Application.AutoMapper;
using Application.Filters;
using Application.Filters.Logger;
using Infra.Contexts;
using Infra.Dapper;
using Infra.UnitOfWork;
using Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using Microsoft.Extensions.FileProviders;
using System.Runtime.Loader;

namespace Api
{

    public class Startup
    {
        private readonly string _environmentName;
        private IConfiguration Configuration { get; }

        private IHostingEnvironment _env;

        public Startup(IHostingEnvironment env)
        {
            var BasePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\_config\"));

            if (env.IsDevelopment())
            {
                BasePath = Directory.GetCurrentDirectory();
            }

            var builder = new ConfigurationBuilder()
              .SetBasePath(BasePath)
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
             .AddEnvironmentVariables();

            Configuration = builder.Build();
            _env = env;
            _environmentName = env.EnvironmentName;
            ConfigureMap.Configure(); //autoMapper
        }


        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            // usando em base de memória
            //ConfigureInMemoryDatabases(services);

            // usando base real
            ConfigureProductionServices(services);
        }

        /// <summary>
        /// Configurações de services de contexto (bd's)
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureProductionServices(IServiceCollection services)
        {
            //Using DbContext (EntityFrameWork)
            // services.AddDbContext<AppContext>(
            //     options => options.UseSqlServer(Configuration["ConnectionStrings:AppConnectString"])
            // );


            // var builder = new DbContextOptionsBuilder<DomainModelPostgreSqlContext>();
            // var connectionString = configuration.GetConnectionString("DataAccessPostgreSqlProvider");
            // builder.UseNpgsql(connectionString);
            // return new DomainModelPostgreSqlContext(builder.Options);


            services.AddDbContext<AppContext>(
                   options => options.UseNpgsql(
                       Configuration["ConnectionStrings:AppConnectString"]));


            //Using Dapper Connection (Only to Strong Queries)
            services.AddDapper(options =>
            {
                options.ConnectionString = Configuration["ConnectionStrings:AppConnectString"];
            });

            // Setting connection with ComputeAPI
            services.AddHttpContextAccessor();

            services.AddHttpClient("ComputeAPI", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = Configuration.GetSection("ComputeAPI:BaseURL").Value;
                client.BaseAddress = new System.Uri(baseURL);
            });

            services.AddHttpClient("IdentityAPI", client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                string baseURL = Configuration.GetSection("IdentityAPI:BaseURL").Value;
                client.BaseAddress = new System.Uri(baseURL);
            });

            ConfigureServices(services);
        }

        /// <summary>
        /// Method to configure the services of the application, hence the registration of them in the application.
        /// They are consumed through the DI(Dependency Injection) or ApplicationServices
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            //Utiliza padrão MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(MidExceptionFilter));
            });
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            //Configurando serviço de documentação do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = $"LABMANAGER API - Ambiente: {_environmentName}",
                    Version = "v1",
                    Description = "API do Sistema LabManager criado com ASP.NET Core",
                    Contact = new Contact
                    {
                        Name = "  ",
                        Email = "",
                        Url = ""
                    }
                });

                //c.TagActionsBy(api => api.HttpMethod); //Se quiser ordenar o swagger somente por HttpMethod (GET,PUT...)

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Favor informar token com bearer no começo separado por espaço", Name = "Authorization", Type = "apiKey" });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });

                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                c.SchemaFilter<IgnorePropertiesSchemaFilter>(); //Para ignorar algumas propriedades no Swagger
                c.IgnoreObsoleteActions();
                c.DescribeAllEnumsAsStrings();

                //Incluindo XML p/ Documentação do Swagger
                string caminhoAplicacao = PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao = PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc = Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");
                c.IncludeXmlComments(caminhoXmlDoc);
            });


            /* 
            => Injeção de Dependência
            - Transient : Criado a cada vez que são solicitados.
            - Scoped: Criado uma vez por solicitação.
            - Singleton: Criado na primeira vez que são solicitados. Cada solicitação subseqüente usa a instância que foi criada na primeira vez.
            */
            services.AddTransient<IUnitOfWork, UnitOfWork<AppContext>>();
            services.AddTransient<ClusterService>();

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            //Utilizando logger (neste caso somente na class filter MmidException p/ logs de erro de aplicação)
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();
            // loggerFactory.AddContext(LogLevel.Error, Configuration.GetConnectionString("LoggerDatabase"));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            /* HABILITANDO CORS */
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.EnableValidator();
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LABMANAGER API");
                c.RoutePrefix = string.Empty;
            });
        }
    }

}