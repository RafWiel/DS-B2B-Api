global using WebApiService.Data;
using WebApiService.Filters;
using WebApiService.Interfaces;
using WebApiService.Middlewares;
//using WebApiService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using WebApiService.Services;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

LogManager.ThrowExceptions = true;
LogManager.ThrowConfigExceptions = true;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers().AddNewtonsoftJson();
    builder.Services.AddCors(u => u.AddDefaultPolicy(x =>
        x.WithOrigins("*")
        .WithExposedHeaders("Content-Disposition")
        .AllowAnyHeader()
        .AllowAnyMethod()));
    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    /*//wylacz wbudowana walidacje modelu
    //builder.Services.Configure<ApiBehaviorOptions>(options =>
    //{
    //    options.SuppressModelStateInvalidFilter = true;
    //});

    ////filtr na poziomie kontrolera
    //builder.Services.AddControllers(u =>
    //{
    //    u.Filters.Add(new ValidateModelFilter());
    //});

    builder.Services.AddScoped<ValidateModelFilter>();*/

    builder.Services.AddScoped<IInstallService, InstallService>();
    builder.Services.AddScoped<IEmployeesService, EmployeesService>();
    builder.Services.AddScoped<ICompaniesService, CompaniesService>();
    builder.Services.AddScoped<ICustomersService, CustomersService>();
    builder.Services.AddScoped<IServiceRequestsService, ServiceRequestsService>();

    builder.Services.AddTransient<GlobalExceptionsMiddleware>();

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors();
    app.MapControllers();
    app.UseMiddleware<GlobalExceptionsMiddleware>();

    logger.Info("Application start");
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}














