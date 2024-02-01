using System.Reflection;
using System.Runtime.CompilerServices;
using Vcr;
using VCR.Demo.IntegrationTests.MaskingVcr;

namespace VCR.Demo.IntegrationTests;

public abstract class VcrTestBase
{
    private readonly Vcr.VCR _vcr;

    protected VcrTestBase()
    {
        var executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
        var testTypeNameWithoutAssemblyNamespace = GetType().FullName!.Replace(executingAssemblyName + ".", "");
        var cassettesDir = new DirectoryInfo($"../../../Cassettes/{testTypeNameWithoutAssemblyNamespace.Replace(".", "/")}");
        _vcr = new Vcr.VCR(new SecretsFileSystemCassetteStorage(cassettesDir, new Dictionary<string, string>
        {
            { "DEMO_KEY", "DEMO_KEY" }
        }));
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