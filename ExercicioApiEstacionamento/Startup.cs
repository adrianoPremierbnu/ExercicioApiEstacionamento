using ExercicioApiEstacionamento.Entidades;
using ExercicioApiEstacionamento.Servicos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExercicioApiEstacionamento
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
               .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
               .AddNewtonsoftJson(c => c.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.Configure<MinhasConfiguracoes>(Configuration.GetSection("Valores"));
            services.AddSingleton<EstacionamentoService>();
            services.AddSingleton<VeiculoService>();
            services.AddSingleton<DiariaService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ExercicioApiEstacionamento", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExercicioApiEstacionamento v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public class MinhasConfiguracoes
        {
            public decimal AbaixoQuinzeMinutosCarro { get; set; }
            public decimal AcimaQuinzeMinutosCarro { get; set; }
            public decimal DiariaCarro { get; set; }
            public decimal DuchaCarro { get; set; }
            public decimal AbaixoQuinzeMinutosMoto { get; set; }
            public decimal AcimaQuinzeMinutosMoto { get; set; }
            public decimal DiariaMoto { get; set; }
            public int TempoLimite { get; set; }
           
        }
                   
    }

}
