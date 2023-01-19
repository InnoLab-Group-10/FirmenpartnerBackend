using CsvHelper.Configuration;
using FirmenpartnerBackend.Configuration;
using FirmenpartnerBackend.Data;
using FirmenpartnerBackend.Mapping;
using FirmenpartnerBackend.Models.Data;
using FirmenpartnerBackend.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using System.Text;

namespace FirmenpartnerBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(ServiceLifetime.Singleton);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FirmenpartnerDB", Version = "v1" });
                c.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.OperationFilter<AuthResponsesOperationFilter>();
            });

            services.AddCors(options =>
            {
                options.AddPolicy("Open", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ModelToResponseProfile>();
                cfg.AddProfile<RequestToModelProfile>();
            });

            // Authentication
            #region Auth

            IConfiguration config = Configuration.GetSection("JwtConfig");
            EMailConfig jwtConfig = config.Get<EMailConfig>();

            byte[]? jwtSecret = Encoding.ASCII.GetBytes(Configuration["JwtConfig:Secret"]);

            services.Configure<IdentityOptions>(options => // Password policy
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddSingleton(jwtConfig);

            TokenValidationParameters? tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };

            services.AddSingleton(tokenValidationParameters);

            TokenValidationParameters? refreshTokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(jwtSecret),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                RequireExpirationTime = true,
            };

            services.AddSingleton(refreshTokenValidationParams);


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<ApiDbContext>();
            #endregion

            // Mailing

            IConfiguration mailConfigSection = Configuration.GetSection("EMailConfig");
            MailConfig mailConfig = mailConfigSection.Get<MailConfig>();

            services.AddSingleton<MailConfig>(mailConfig);

            services.AddMailKit(optionBuilder =>
            {
                optionBuilder.UseMailKit(new MailKitOptions()
                {
                    Server = mailConfig.Server,
                    Port = mailConfig.Port,
                    SenderName = mailConfig.SenderName,
                    SenderEmail = mailConfig.SenderMail,
                    Account = mailConfig.Username,
                    Password = mailConfig.Password,
                    Security = true
                });
            });

            // Custom services

            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddScoped<IMailSettingsService, MailSettingsService>();
            services.AddScoped<ITemplateMailService, TemplateMailService>();

            // Configuration

            CsvConfiguration csvConfiguration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };

            services.AddSingleton(csvConfiguration);

            IConfiguration uploadConfigSection = Configuration.GetSection("FileUploadConfig");
            FileUploadConfig uploadConfig = uploadConfigSection.Get<FileUploadConfig>();

            services.AddSingleton(uploadConfig);

            // Create needed directories for file uploads

            if (!Directory.Exists(uploadConfig.TargetPath)) Directory.CreateDirectory(uploadConfig.TargetPath);
        }


        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FirmenpartnerDB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseCors("Open");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.ApplicationServices.GetRequiredService<ApiDbContext>().Database.Migrate();

            // Default root user and roles
            using (var scope = serviceProvider.CreateScope())
            {
                await CreateDefaultMailSettings(scope.ServiceProvider);
                await CreateRoles(scope.ServiceProvider);
                await CreateRootUser(scope.ServiceProvider, Configuration.GetSection("RootUserConfig").Get<RootUserConfig>());
            }
        }

        private async Task CreateDefaultMailSettings(IServiceProvider serviceProvider)
        {
            ApiDbContext dbContext = serviceProvider.GetRequiredService<ApiDbContext>();

            foreach (var setting in MailSettings.DefaultMailSettings)
            {
                MailSetting? entry = await dbContext.MailSettings.FindAsync(setting.Id);

                if (entry == null)
                {
                    dbContext.Add(setting);
                }
            }

            dbContext.SaveChanges();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            RoleManager<IdentityRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityResult roleResult;

            // Find user roles using reflection (not performant but whatever)
            List<string> roles = new List<string>();
            foreach (System.Reflection.FieldInfo? roleField in typeof(ApplicationRoles).GetFields())
            {
                if (roleField.IsLiteral && roleField.FieldType == typeof(string))
                {
                    roles.Add((string)roleField.GetRawConstantValue());
                }
            }

            // Add the roles
            foreach (string? roleName in roles)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private async Task CreateRootUser(IServiceProvider serviceProvider, RootUserConfig config)
        {
            UserManager<ApplicationUser>? userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser? rootUser = new ApplicationUser
            {
                UserName = config.Username,
                Email = config.Email,
            };

            ApplicationUser? existingUser = await userManager.FindByEmailAsync(config.Email);

            if (existingUser == null)
            {
                IdentityResult? createRootUser = await userManager.CreateAsync(rootUser, config.Password);
                if (createRootUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(rootUser, ApplicationRoles.USER);
                    await userManager.AddToRoleAsync(rootUser, ApplicationRoles.ADMIN);
                    await userManager.AddToRoleAsync(rootUser, ApplicationRoles.ROOT);
                }
            }
        }
    }
}
