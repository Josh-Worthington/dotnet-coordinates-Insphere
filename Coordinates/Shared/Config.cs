using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared;

/// <summary>
/// 	The shared configuration.
/// </summary>
[JsonSerializable(typeof(Config), GenerationMode = JsonSourceGenerationMode.Default)]
public record Config
{
	public static string RootDirectory => Environment.CurrentDirectory.Contains("Debug")
											  ? Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\.."))
											  : Environment.CurrentDirectory;

	private readonly JsonSerializerOptions _serializerOptions = new()
	{
		WriteIndented = true
	};
	private readonly string _path;
	private int _port;

	public Config()
	{
		_path = Path.Combine(RootDirectory, "settings.json");
		_serializerOptions.Converters.Add(new JsonStringEnumConverter());
	}

	/// <summary>
	/// 	Gets or sets the port.
	/// </summary>
	public int Port
	{
		get => _port;
		set
		{
			_port = value;

			var newJson = JsonSerializer.Serialize(this, _serializerOptions);
			using var writer = new StreamWriter(new FileStream(_path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite));
			writer.Write(newJson);
		}
	}


}