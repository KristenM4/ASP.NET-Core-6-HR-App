using Microsoft.EntityFrameworkCore;
using SeaWolf.HR.Models;
using System.Text.Json.Serialization;
using Serilog;
using Serilog.Events;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using Newtonsoft.Json;
using Microsoft.OpenApi.Models;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .WriteTo.Seq(serverUrl: "http://seq:5341")
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddIdentity<HRUser, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
    })
        .AddEntityFrameworkStores<SeaWolfHRDbContext>();

    builder.Services.AddAuthentication()
        .AddCookie()
        .AddJwtBearer(cfg =>
        {
            cfg.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = builder.Configuration["Tokens:Issuer"],
                ValidAudience = builder.Configuration["Tokens:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Tokens:Key"]))
            };
        });

    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<ILocationRepository, LocationRepository>();

    builder.Services.AddControllersWithViews(options =>
        {
            options.ReturnHttpNotAcceptable = true;
        })
        .AddNewtonsoftJson(options => 
        { 
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; 
        })
        .AddXmlDataContractSerializerFormatters()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(setupAction =>
    {
        var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

        setupAction.IncludeXmlComments(xmlCommentsFullPath);

        setupAction.AddSecurityDefinition("SeaWolfHRApiBearerAuth", new OpenApiSecurityScheme()
        {
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            Description = "Input valid token to use this API"
        });

        setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "SeaWolfHRApiBearerAuth" }
                }, new List<string>() }
        });
    });

    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

    builder.Services.AddApiVersioning(setupAction =>
    {
        setupAction.AssumeDefaultVersionWhenUnspecified = true;
        setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
        setupAction.ReportApiVersions = true;
    });

    builder.Services.AddDbContext<SeaWolfHRDbContext>(options => {
        options.UseSqlServer(
            builder.Configuration["ConnectionStrings:SeaWolfHRDbContextConnection"]);
    });

    builder.Host.UseSerilog();

    var app = builder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=App}/{action=Index}/{id?}");

    using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        var db = serviceScope.ServiceProvider.GetRequiredService<SeaWolfHRDbContext>().Database;
        while (!db.CanConnect())
        {
            Thread.Sleep(1000);
        }
        serviceScope.ServiceProvider.GetRequiredService<SeaWolfHRDbContext>().Database.Migrate();

    }


    DbSeeder.SeedAsync(app).Wait();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}