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
        .AddNewtonsoftJson()
        .AddXmlDataContractSerializerFormatters()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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