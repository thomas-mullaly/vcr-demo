using Vcr;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace VCR.Demo.IntegrationTests.MaskingVcr;

public class SecretsFileSystemCassetteStorage : ICasseteStorage
{
    private readonly DirectoryInfo _storageLocation;
    private readonly Dictionary<string, string> _replacements = new();

    public SecretsFileSystemCassetteStorage(DirectoryInfo storageLocation)
    {
        _storageLocation = storageLocation;
        _storageLocation.Create();
    }

    public void AddSecretReplacement(string key, string value)
    {
        _replacements[key] = value;
    }

    public List<HttpInteraction>? Load(string name)
    {
        var safeFileName = GetSafeFileName(name);
        var file = _storageLocation.GetFiles().SingleOrDefault(f => f.Name == safeFileName);

        if (file != null)
        {
            return LoadFile(file);
        }

        return null;
    }

    public void Save(string name, IEnumerable<HttpInteraction> httpInteractions)
    {
        var interactions = httpInteractions.ToList();

        var safeName = GetSafeFileName(name);
        var file = new FileInfo(Path.Combine(_storageLocation.FullName, safeName));

        using var stream = file.Open(FileMode.OpenOrCreate, FileAccess.Write);
        using var writer = new StreamWriter(stream);

        var serializer = new SerializerBuilder()
            .DisableAliases()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .Build();

        var contents = serializer.Serialize(new StorageWrapperV1 { HttpInteractions = interactions });

        foreach (var replacement in _replacements)
        {
            contents = contents.Replace(replacement.Key, $"{{REDACTED_{replacement.Value.ToUpper()}}}");
        }
        
        writer.Write(contents);
        writer.Close();
    }

    private List<HttpInteraction> LoadFile(FileInfo file)
    {
        using var stream = file.OpenRead();
        using var reader = new StreamReader(stream);

        var contents = reader.ReadToEnd();

        foreach (var replacement in _replacements)
        {
            contents = contents.Replace($"{{REDACTED_{replacement.Value.ToUpper()}}}", replacement.Key);
        }

        var serializer = new DeserializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .Build();

        return serializer.Deserialize<StorageWrapperV1>(contents).HttpInteractions.ToList();
    }

    private string GetSafeFileName(string name)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(invalidChar, '_');
        }

        return name + ".yaml";
    }

    private class StorageWrapperV1
    {
        [YamlMember(Order = 1)]
        public string Version { get; set; } = "VCR.net 1.0.0";
        [YamlMember(Order = 2)]
        public List<HttpInteraction> HttpInteractions { get; set; } = default!;
    }
}