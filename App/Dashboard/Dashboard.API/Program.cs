using Dashboard.DataAccess;
using Dashboard.DataAccess.Context;
using Dashboard.BussinessLogic;
using Dashboard.Common.Options;
using Microsoft.EntityFrameworkCore;
using Dashboard.API.Middleware;
using Dashboard.ServiceDefaults;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EncryptionOptions>(builder.Configuration.GetSection("Encryption"));
builder.Services.Configure<EmailOptions>(builder.Configuration.GetSection("Email"));
builder.Services.Configure<StockWorkerOptions>(builder.Configuration.GetSection("StockWorker"));
builder.Services.Configure<FinancialReportingOptions>(builder.Configuration.GetSection("FinancialReporting"));
builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));

builder.AddServiceDefaults();
builder.AddDataAccess();

builder.AddBussinessLogicServices();

builder.Services.AddDbContext<WebbanhangDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebbanhangDB")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<WebbanhangDbContextInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();