using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace AiCommander;

internal class ApiRouteBuilder {

    private StringBuilder Builder {get;}

    public ApiRouteBuilder(IPAddress addr, int port) {
        Builder = new();

        Builder.Append($"http://{addr.ToString()}:{port}/api");
    }

    public ApiRouteBuilder WithEndpoint(string endpoint) {
        Builder.Append(endpoint);
        return this;
    }

    public string BuildUrl() => Builder.ToString();
}

internal interface ArgsModule {

    public IPAddress Address {get;}
    public int Port {get;}

    public abstract static ArgsModule Parse(string[] args);

    public Task<bool> ExecuteAsync();    
}

internal static class ArgsParserHelper {
    public static (IPAddress, int) GetBaseConfig(string[] args) {
        IPAddress addr = IPAddress.Loopback;
        int port = 7120;
        for (int i = 0; i < args.Length; i++){
            if (args[i] == "--host") {
                addr = IPAddress.Parse(args[i+1]);
            }
            if (args[i] == "-p") {
                port = int.Parse(args[i+1]);
            }
            i += 1;
        }
        return (addr, port);
    }
}

internal struct ListModule : ArgsModule
{
    private static string Endpoint => "/list/collections/";

    public IPAddress Address {get;}

    public int Port {get;}

    public ListModule(IPAddress address, int port) {
        Address = address;
        Port = port;
    }

    public static ArgsModule Parse(string[] args)
    {
        (IPAddress addr, int port) = ArgsParserHelper.GetBaseConfig(args);
        return new ListModule(addr, port);
    }

    public async Task<bool> ExecuteAsync()
    {
        string url = new ApiRouteBuilder(Address, Port).WithEndpoint(Endpoint).BuildUrl();
        Console.WriteLine(url);
        HttpClient client = new HttpClient();

        HttpResponseMessage res = await client.GetAsync(url);
        
        if (res.IsSuccessStatusCode) {
            List<string>? collections = await res.Content.ReadFromJsonAsync<List<string>>();
            if (collections is null) {
                Console.WriteLine("Could not parse collection!");
                return false;
            }
            foreach (string col in collections) {
                Console.WriteLine(col);
            }

            return true;
        }

        Console.WriteLine("Request was not successful!");
        return false;
    }
}

internal struct EmbedModule : ArgsModule
{
    public IPAddress Address => throw new NotImplementedException();

    public int Port => throw new NotImplementedException();

    public static ArgsModule Parse(string[] args)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}

internal struct AskModule : ArgsModule
{
    public IPAddress Address => throw new NotImplementedException();

    public int Port => throw new NotImplementedException();

    public static ArgsModule Parse(string[] args)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExecuteAsync()
    {
        throw new NotImplementedException();
    }
}

enum CommandType {
    Interactive,
    Question,
    Embed,
    Topic,
    List,
}

static class CommandTypeExt {
    public static bool HasContent(this CommandType self) => self switch {
        CommandType.Interactive => false,
        CommandType.Question => true,
        CommandType.Embed => true,
        CommandType.Topic => true,
        CommandType.List => false,
        _ => throw new NotSupportedException("Command type is not supported!")
    }; 
}

static class StringExtensions {

    public static CommandType ToCommandType(this string self) => self switch {
        "-i" => CommandType.Interactive,
        "-q" => CommandType.Question,
        "-e" => CommandType.Embed,
        "-t" => CommandType.Topic,
        "-l" => CommandType.List,
        _ => throw new ArgumentException("Invalid command!")
    };
}

class Program
{
    
    static async Task Main(string[] args)
    {
        ArgsModule? module = null;

        switch (args[0]) {
            case "inter":
                Console.WriteLine("Currently not supported!");
                break;
            case "ask":
                module = AskModule.Parse(args);
                break;
            case "embed":
                module = EmbedModule.Parse(args);
                break;
            case "list":
                module = ListModule.Parse(args);
                break;
            default:
                Console.WriteLine("This module is not supported!");
                break;
        }

        await module?.ExecuteAsync();
    }
}
