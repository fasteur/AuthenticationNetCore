
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AuthenticationNetCore.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using AuthenticationNetCore.Api.Services.EmailSenderService;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Services.Teachers.TeacherService;
using AuthenticationNetCore.Api.Repositories.ClasseRepo;
using AuthenticationNetCore.Api.Repositories.Teachers.TeacherRepo;
using AuthenticationNetCore.Api.Repositories.Teachers.AuthTeacherRepo;
using AuthenticationNetCore.Api.Repositories.Students.AuthStudentRepo;
using AuthenticationNetCore.Api.Services.Teachers.AuthTeacherService;
using AuthenticationNetCore.Api.Services.Students.AuthStudentService;
using AuthenticationNetCore.Api.Repositories.Students.StudentRepo;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AuthenticationNetCore.Api
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
            services.AddDbContext<DataContext>(x => x
                .UseLoggerFactory(LoggerFactory.Create(b => b.AddConsole()))
                .UseSqlServer(Configuration["Auth:ConnectionStrings"])
            );
            services.AddAutoMapper(typeof(Startup));

            services.AddCors(options =>
            {
                options.AddPolicy("DevelopPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader();
                    }
                );
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyMethod().AllowAnyHeader();
                    }
                );
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Auth:Token"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddAuthorization(config =>
            {
                var userAuthPolicyBuilder = new AuthorizationPolicyBuilder();
                config.DefaultPolicy = userAuthPolicyBuilder
                                    .RequireAuthenticatedUser()
                                    .RequireClaim(ClaimTypes.Role)
                                    .RequireClaim(ClaimTypes.NameIdentifier)
                                    .Build();
            });
            services.AddControllers();
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Teachers
            services.AddScoped<IAuthTeacherRepository, AuthTeacherRepository>();
            services.AddScoped<IAuthTeacherService, AuthTeacherService>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ITeacherService, TeacherService>();
            
            // Students
            services.AddScoped<IAuthStudentRepository, AuthStudentRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IAuthStudentService, AuthStudentService>();

            services.AddScoped<IClasseRepository, ClasseRepository>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);
            // services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("DevelopPolicy");
            }
            else
            {
                app.UseCors();
                app.UseExceptionHandler("/Error");

            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
