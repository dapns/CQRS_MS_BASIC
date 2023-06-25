using AuthIdentity.BLL.Mapper;
using AuthIdentity.BLL.Middleware;
using AuthIdentity.BLL.Queries;
using AuthIdentity.DAL.Context;
using AutoMapper;
using CORE.Repository;
using CORE.Repository.UOW.DI;
using MediatR;
using System.Reflection;

namespace AuthIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // add services to DI container
            {               
                var services = builder.Services;

                // Add services to the container.
                services.AddCors();
                services.AddControllers();
                services.AddControllers().AddNewtonsoftJson();
                //services.AddAuthorization();

                // configure strongly typed settings object
                //services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


                //services.AddMediatR(Assembly.GetExecutingAssembly());
                services.AddMediatR(typeof(UserLoginQuery).GetTypeInfo().Assembly,
                                    typeof(GetUserByIdQuery).GetTypeInfo().Assembly);
                services.AddDbContext<AuthIdentityContext>();
                services.AddUnitOfWork<AuthIdentityContext>();
                
               
                //automapper di
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new UserMapper());
                    //mc.AddProfile(new LeftMenuMapper());
                });

                IMapper mapper = mapperConfig.CreateMapper();
                services.AddSingleton(mapper);
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            {
                //app.UseHttpsRedirection();

               

                //app.UseAuthentication();
                // custom jwt auth middleware
               
                //app.UseRouting();
                // global cors policy
                app.UseCors(x => x
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

                //app.UseAuthorization();
                app.UseMiddleware<AuthenticationMiddleware>();
                app.UseAuthorization();
                app.UseMiddleware<JwtMiddleware>();
                //app.UseEndpoints(endpoints =>
                //{
                //    endpoints.MapControllers();
                //});

                app.MapControllers();

                app.Run();
            }
        }
    }
}