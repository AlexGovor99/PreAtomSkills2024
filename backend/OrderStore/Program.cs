using Microsoft.EntityFrameworkCore;
using OrderStore.Application.Services;
using OrderStore.Core.Abstractions;
using OrderStore.DataAccess;
using OrderStore.DataAccess.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderStoreDbContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration["ConnectionString"]);
    });
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrdersRepository, OrdersRepository>();
builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddHostedService<BackgroundApprovalService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<OrderStoreDbContext>();
    context.Database.Migrate();
}


app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins("http://localhost:3000");
    x.WithMethods().AllowAnyMethod();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();

