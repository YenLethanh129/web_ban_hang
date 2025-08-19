using Dashboard.DataAccess;
using Dashboard.DataAccess.Context;
using Dashboard.BussinessLogic; 
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddDataAccess();

builder.AddApplicationServices();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
