using Core.Interfaces;
using Core.Specification;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<StoredContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>) , typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(ISpecification<>) , typeof(BaseSpecifications<>));


var app = builder.Build();


app.MapControllers();

try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<StoredContext>();
    await context.Database.MigrateAsync();
    await StoredContextSeed.SeedAsync(context);
}
catch(Exception ex)
{
    Console.WriteLine(ex);
}

app.Run();
