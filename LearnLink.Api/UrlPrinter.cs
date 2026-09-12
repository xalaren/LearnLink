using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace LearnLink.Api;

/// <summary>
/// Provides URL printing to the console
/// </summary>
/// <param name="server">Respective server</param>
public class UrlPrinter(IServer server)
{
    private static readonly TimeSpan ServerTimeout = TimeSpan.FromMinutes(1);
    private readonly IServer _server = server;

    /// <summary>
    /// Executes task of URL printer
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task Start(CancellationToken cancellationToken)
    {
        var url = await GetUrlWhenServerIsReady(cancellationToken);
        PrintUrl(url);
    }

    private async Task<string> GetUrlWhenServerIsReady(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        IServerAddressesFeature? addresses;
        do
        {
            addresses = _server.Features.Get<IServerAddressesFeature>();

            if (addresses!.Addresses.Count > 0)
                break;

            await Task.Delay(100, cancellationToken);
        } while (sw.Elapsed < ServerTimeout);

        return addresses.Addresses.Select(x => x.Replace("[::]", "localhost").Replace("+:", "localhost:")).Single();
    }

    private static void PrintUrl(string url)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{Assembly.GetExecutingAssembly().GetName().Name} is started on ");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(url);

        Console.ResetColor();
    }
}
