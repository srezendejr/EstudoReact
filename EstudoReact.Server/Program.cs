using EstudoReact.Data;
using EstudoReact.Server.Mapping;
using EstudoReact.Service;
using EstudoReact.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("https://localhost:58465/")
                   .AllowAnyHeader()
                   .AllowAnyOrigin()
                   .AllowAnyMethod();
        });
});
builder.Services.AddScoped<CompradorService>();
builder.Services.AddScoped<CidadeService>();
builder.Services.AddScoped<EstadoService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

var connectionString = builder.Configuration.GetConnectionString("BancoDados");

builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();

