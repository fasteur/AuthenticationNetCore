using System.Security.Claims;
using System.Text;
using AuthenticationNetCore.Api.Data;
using AuthenticationNetCore.Api.Models;
using AuthenticationNetCore.Api.Repositories.ClasseRepo;
using AuthenticationNetCore.Api.Repositories.Students.AuthStudentRepo;
using AuthenticationNetCore.Api.Repositories.Students.StudentRepo;
using AuthenticationNetCore.Api.Repositories.Teachers.AuthTeacherRepo;
using AuthenticationNetCore.Api.Repositories.Teachers.TeacherRepo;
using AuthenticationNetCore.Api.Services.EmailSenderService;
using AuthenticationNetCore.Api.Services.Students.AuthStudentService;
using AuthenticationNetCore.Api.Services.Teachers.AuthTeacherService;
using AuthenticationNetCore.Api.Services.Teachers.TeacherService;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;

namespace AuthenticationNetCore.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddDbContext<DataContext> (x => x
                .UseLoggerFactory (LoggerFactory.Create (b => b.AddConsole ()))
                .UseSqlServer (Configuration["Auth:ConnectionStrings"])
            );
            services.AddAutoMapper (typeof (Startup));

            services.AddCors (options => 
            {
                options.AddPolicy ("DevelopPolicy",
                    builder => {
                        builder.AllowAnyMethod ().AllowAnyOrigin ().AllowAnyHeader ();
                    }
                );
                options.AddDefaultPolicy (
                    builder => {
                        builder.AllowAnyMethod ().AllowAnyHeader ();
                    }
                );
            });

            services.AddAuthentication (JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer (options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (Configuration["Auth:Token"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                    };
                });
            services.AddAuthorization (config => {
                var userAuthPolicyBuilder = new AuthorizationPolicyBuilder ();
                config.DefaultPolicy = userAuthPolicyBuilder
                    .RequireAuthenticatedUser ()
                    .RequireClaim (ClaimTypes.Role)
                    .RequireClaim (ClaimTypes.NameIdentifier)
                    .Build ();
            });
            services.AddControllers ();
            services.AddMvc (option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion (CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson (opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            // todo: check jsonparsor

            services.AddSwaggerGen (c => {
                c.SwaggerDoc ("v1", new OpenApiInfo {
                    Version = "v1",
                        Title = "AuthenticationNetCore API",
                        Description = "Manage your class of students",
                        Contact = new OpenApiContact {
                            Name = "Florian Asteur",
                            Email = "asteur.florian@gmail.com"
                        }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.StaticWebAssets.xml";
                var xmlPath = Path.Combine (AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
            // Teachers
            services.AddScoped<IAuthTeacherRepository, AuthTeacherRepository> ();
            services.AddScoped<IAuthTeacherService, AuthTeacherService> ();
            services.AddScoped<ITeacherRepository, TeacherRepository> ();
            services.AddScoped<ITeacherService, TeacherService> ();

            // Students
            services.AddScoped<IAuthStudentRepository, AuthStudentRepository> ();
            services.AddScoped<IStudentRepository, StudentRepository> ();
            services.AddScoped<IAuthStudentService, AuthStudentService> ();

            services.AddScoped<IClasseRepository, ClasseRepository> ();
            services.AddTransient<IEmailSender, EmailSender> ();
            services.Configure<AuthMessageSenderOptions> (Configuration);
            // services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {

            app.UseRouting ();

            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
                app.UseCors ("DevelopPolicy");
            } else {
                app.UseCors ();
                app.UseExceptionHandler ("/Error");
                app.UseHsts();
            }

            app.UseSwagger ();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseAuthentication ();
            app.UseAuthorization ();

            app.UseEndpoints (endpoints => {
                endpoints.MapControllers ();
            });
        }
    }
}
