using CoordinateReader.GrpuServices;
using CoordinateReader.Interfaces.Services;
using CoordinateReader.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ReaderService>();
if (app.Environment.IsDevelopment())
{
	app.MapGrpcReflectionService();
}

app.Run();
