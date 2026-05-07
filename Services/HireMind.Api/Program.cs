var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins") // Angular app URL
    .Get<string[]>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

builder.Services.AddHttpContextAccessor();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add UnitOfWork and Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IBusinessCardRepository, BusinessCardRepository>();
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.AddScoped<IAnalyzeCvRepository, AnalyzeCvRepository>();
builder.Services.AddScoped<IHiringStageRepository, HiringStageRepository>();
builder.Services.AddScoped<IApplicationStageRepository, ApplicationStageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IAboutUsRepository, AboutUsRepository>();
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings.Secret)),

        ClockSkew = TimeSpan.Zero
    };
    //options.Events = new JwtBearerEvents
    //{
    //    OnTokenValidated = async context =>
    //    {
    //        var userId = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
    //        var tokenVersionClaim = context.Principal?.FindFirst("tokenVersion")?.Value;

    //        if (userId == null || tokenVersionClaim == null)
    //        {
    //            context.Fail("Invalid token");
    //            return;
    //        }

    //        var userRepo = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

    //        var user = await userRepo.GetUserById(Guid.Parse(userId), CancellationToken.None);

    //        if (user == null)
    //        {
    //            context.Fail("User not found");
    //            return;
    //        }

    //        if (user.TokenVersion.ToString() != tokenVersionClaim)
    //        {
    //            context.Fail("Token revoked");
    //            return;
    //        }
    //    }
    //};
});

builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddTransient(sp =>
{
    return new SmtpClient("smtp.gmail.com")
    {
        Port = 587,
        Credentials = new System.Net.NetworkCredential("moathselawe2@gmail.com", "bovy 7ff7 ddfa dpbo"),
        EnableSsl = true
    };
});

builder.Services.AddTransient<INotificationService, NotificationService>();

builder.Services.Configure<EmailVerificationSettings>(
    builder.Configuration.GetSection("EmailVerification"));

builder.Services.AddHttpClient<IAIService, AIService>(client =>
{
    client.BaseAddress = new Uri("https://openrouter.ai/");
});
builder.Services.AddHttpClient<IAnalyzeCvService, AnalyzeCvService>(client =>
{
    client.BaseAddress = new Uri("https://openrouter.ai/");
});

// Add MediatR and scan all assemblies for handlers
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
});

// Add FluentValidation: scan all assemblies for validators
builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

// Add FluentValidation pipeline for MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Other services
builder.Services.AddScoped<IFileParserService, FileParserService>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "HireMind API V1");
        c.RoutePrefix = string.Empty; 
    });
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await RoleSeeder.SeedAsync(db);
    await PermissionSeeder.SeedAsync(db);
    await RoleSeeder.AssignAdminPermissions(db);
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
