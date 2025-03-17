using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using ProvaPub.Application.Common.Interfaces;
using ProvaPub.Application.Common.Mappings;
using ProvaPub.Application.Common.Providers;
using ProvaPub.Application.Factories;
using ProvaPub.Application.Services;
using ProvaPub.Domain.Interfaces.Factories;
using ProvaPub.Domain.Interfaces.Services;
using ProvaPub.Infrastructure.Data;

var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

try
{
    logger.Info("Iniciando a aplicação ProvaPub.");

    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

    builder.Services.AddScoped<IRandomService, RandomService>();
    builder.Services.AddScoped<ICustomerService, CustomerService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddScoped<IPaymentMethodFactory, PaymentMethodFactory>();
    builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
    builder.Services.AddScoped<PixPaymentService>();
    builder.Services.AddScoped<CreditCardPaymentService>();
    builder.Services.AddScoped<PayPalPaymentService>();

    builder.Services.AddDbContext<TestDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ctx")));
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Erro ao iniciar a aplicação ProvaPub.");
    throw;
}
finally
{
    LogManager.Shutdown();
}

