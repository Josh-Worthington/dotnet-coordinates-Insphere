using CoordinateReader.Interfaces.Services;
using CoordinateReader.Services;
using Shared;

namespace CoordinateReader;

/// <summary>
/// 	The program.
/// </summary>
public class Program
{
	/// <summary>
	/// 	Main entry-point for this application.
	/// </summary>
	public static void Main(
		params string[] _)
	{
		var builder = WebApplication.CreateBuilder();

		var sharedConfig = new ConfigurationServiceSingleton()
			.Config;

		var url = new UriBuilder
		{
			Scheme = "https",
			Host = "localhost",
			Port = sharedConfig.Port
		}.ToString();
		builder.WebHost.UseUrls(url);

		// Add services to the container.
		builder.Services.AddGrpc();
		builder.Services.AddGrpcReflection();
		builder.Services.AddScoped<ICsvReaderService, CsvReaderService>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		app.MapGrpcService<ReaderService>();
		app.MapGrpcService<PingService>();

		if (app.Environment.IsDevelopment())
		{
			app.MapGrpcReflectionService();
		}

		app.Run();
	}
}
