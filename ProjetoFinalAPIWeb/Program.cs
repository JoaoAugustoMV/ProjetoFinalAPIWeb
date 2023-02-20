using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using ProjetoFinalAPIWeb.Filtros;
using ProjetoFinalAPIWeb.Infra.Data.Repository;
using ProjetoFinalAPIWeb.Repository;
using ProjetoFinalAPIWeb.Service.Interface;
using ProjetoFinalAPIWeb.Service.Service;

namespace ProjetoFinalAPIWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            //builder.Services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao"), p => p.EnableRetryOnFailure());
            //});

            //.ConfigureApiBehaviorOptions(options =>
            //{
            //    options.SuppressModelStateInvalidFilter = true;
            //}); ;

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExececaoGeralFilter));
            }).AddJsonOptions(options => {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;                
                });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            byte[] key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("key"));
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true, // para inativar a validação do issuer, informar false e remover ValidIssuer
                        ValidateAudience = true, // para inativar a validação da audience, informar false e remover ValidAudience
                        ValidIssuer = "APIClientes.com",
                        ValidAudience = "APIEvents.com"
                    };
                });

            builder.Services.AddScoped<ICityEventRepository, CityEventRepository>();
            builder.Services.AddScoped<ICityEventService, CityEventService>();
            builder.Services.AddScoped<IEventReservationRepository, EventReservationRepository>();
            builder.Services.AddScoped<IEventReservationService, EventReservationService>();

            // Filtros 
            builder.Services.AddScoped<ValidarDataEventoFilter>();
            builder.Services.AddScoped<ValidarPrecoFilter>();        
            

            var app = builder.Build();

            if(app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}