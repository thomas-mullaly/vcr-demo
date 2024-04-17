using System.Reflection;
using System.Runtime.CompilerServices;
using Vcr;
using VCR.Demo.IntegrationTests.MaskingVcr;

namespace VCR.Demo.IntegrationTests;

public abstract class VcrTestBase
{
    private readonly Vcr.VCR _vcr;
    private readonly SecretsFileSystemCassetteStorage _cassetteStorage;
    
    protected VcrTestBase()
    {
        var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
        var testTypeNameWithoutAssemblyNamespace = GetType().FullName!.Replace(executingAssemblyName + ".", "");
        var cassettesDirectory = new DirectoryInfo($"../../../Cassettes/{testTypeNameWithoutAssemblyNamespace.Replace(".", "/")}");
        _cassetteStorage = new SecretsFileSystemCassetteStorage(cassettesDirectory);
        _vcr = new Vcr.VCR(_cassetteStorage);
    }

    protected void AddSecretsReplacement(string valueToReplace, string replacementValue)
    {
        _cassetteStorage.AddSecretReplacement(valueToReplace, replacementValue);
    }

    protected HttpClient CreateClient()
    {
        var vcrHandler = _vcr.GetVcrHandler();
        vcrHandler.InnerHandler = new SocketsHttpHandler();

        var client = new HttpClient(vcrHandler);

        return client;
    }

    protected Cassette UseCassette([CallerMemberName] string? testMethod = null)
    {
        return _vcr.UseCassette(testMethod);
    }
}