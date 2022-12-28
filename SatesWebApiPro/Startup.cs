using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SatesWebApiPro.Data;
using SatesWebApiPro.Interfaces;
using SatesWebApiPro.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SatesWebApiPro
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
            services.AddDbContext<ProductContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("Local"));
            });
            //DI Register edildi
            services.AddScoped<IProductRepository, ProductRepository>();
            //IProductRepository gördüðün zaman ProductRepository örneðini ver
            services.AddCors(cors =>
            {
                cors.AddPolicy("UdemyCorsPolicy",opt=> 
                {
                    //her türlü originden gelen istek kabul olacak
                    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    //withorigin ile belli sitelere api ni açabilirsin
                });
            });
            //jsonlarda çakýþmayý önlemek için  AddNewtonsoftJson kullan
            services.AddControllers().AddNewtonsoftJson(opt=> {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SatesWebApiPro", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SatesWebApiPro v1"));
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("UdemyCorsPolicy");

            app.UseAuthorization();
            //controller ile maple
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
