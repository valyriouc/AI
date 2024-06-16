using System.Net;
using System.Net.Http.Json;
using System.Reflection;
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

    public abstract static ArgsModule? Parse(string[] args);

    public Task<bool> ExecuteAsync();    
}

internal static class ArgsParserHelper {
    public static (IPAddress, int) GetBaseConfig(string[] args) {
        IPAddress addr = IPAddress.Loopback;
        int port = 7120;
        for (int i = 0; i < args.Length; i++){
            if (args[i] == "--host") {
                addr = IPAddress.Parse(args[i+1]);
                i += 1;
            }
            if (args[i] == "-p") {
                port = int.Parse(args[i+1]);
                i += 1;
            }
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

    public static ArgsModule? Parse(string[] args)
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
                Console.WriteLine("[ERROR] Could not parse collection!");
                return false;
            }
            foreach (string col in collections) {
                Console.WriteLine(col);
            }

            return true;
        }

        Console.WriteLine("[ERROR] Request was not successful!");
        return false;
    }
}

internal struct EmbedModule : ArgsModule
{   
    private static string Endpoint => "/embed/";

    public IPAddress Address {get;}

    public int Port {get;}  

    private string topic;
    private string filename;

    public EmbedModule(IPAddress addr, int port, string topic, string filename) {
        Address = addr;
        Port = port;
        this.topic = topic;
        this.filename = filename;
    }

    public static ArgsModule? Parse(string[] args)
    {
        (IPAddress addr, int port) = ArgsParserHelper.GetBaseConfig(args);

        string? topic = null;
        string? filename = null;

        Console.WriteLine(args.Length);

        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-t") {
                Console.WriteLine(args[i+1]);
                topic = args[i+1];
                i += 1;
            }
            if (args[i] == "-f") {
                filename = args[i+1];
                i += 1;
            }

        }

        if (topic is null) {
            Console.WriteLine("Specify the topic by using -t!");
            return null;
        }

        if (filename is null) {
            Console.WriteLine("Specify the filepath by using -f!");
            return null;
        }

        return new EmbedModule(addr, port, topic, filename);
    }

    struct Success {
        public string Status {get; set;}

        public string Msg {get; set;}

    }

    public async Task<bool> ExecuteAsync()
    {
        string current = Directory.GetCurrentDirectory();
        string filepath = Path.Combine(current, this.filename);

        if (!File.Exists(filepath)) {
            System.Console.WriteLine("Specified file for embedding does not exists!");
            return false;
        }

        byte[] raw = await File.ReadAllBytesAsync(filepath);
    
        string url = new ApiRouteBuilder(Address, Port).WithEndpoint(Endpoint).BuildUrl();
        url += $"?topic={this.topic}";
        
        string? assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (assemblyDir is null) {
            throw new DirectoryNotFoundException("Something went wrong retrieving the executing directory!");
        }

        string storedEmbed = Path.Combine(assemblyDir, "embedded.txt");
        if (!File.Exists(storedEmbed)) {
            File.Create(storedEmbed).Dispose();
        }

        string[] embedded = File.ReadAllLines(storedEmbed).Select(x => x.Trim()).ToArray();
        if (embedded.Contains(filepath)) {
            Console.WriteLine("Document is already embedded!");
            return false;
        }
        File.AppendAllLines(storedEmbed, [ filepath ]);

        HttpClient client = new();

        ByteArrayContent content = new ByteArrayContent(raw);

        HttpResponseMessage res = await client.PostAsync(url, content);

        if (res.IsSuccessStatusCode) {
            Success success = await res.Content.ReadFromJsonAsync<Success>();
            Console.WriteLine(success.Msg);
            return true;
        }

        Console.WriteLine("The request was not successful!");
        return false;
    }
}

internal struct AskModule : ArgsModule
{
    private static string Endpoint => "/";

    public IPAddress Address {get;}

    public int Port {get;}

    private string questions {get;}
    private string topic {get;}

    public AskModule(IPAddress addr, int port, string quest, string top) {
        Address = addr;
        Port = port;
        questions = quest;
        topic = top;
    }

    struct Payload {
        public string Topic {get; set;}
        public string Question {get; set; }
    }

    public static ArgsModule? Parse(string[] args)
    {
        (IPAddress addr, int port) = ArgsParserHelper.GetBaseConfig(args);

        string? topic = null;
        string? question = null;
        for (int i = 0; i < args.Length; i++) {
            if (args[i] == "-t") {
                topic = args[i+1];
                i += 1;
            }
            if (args[i] == "-q") {
                question = args[i+1];
                i += 1;
            }
        }

        if (topic is null) {
            Console.WriteLine("Specify the topic with -t");
            return null;
        }

        if (question is null) {
            Console.WriteLine("Specify the question with -q");
            return null;
        }

        return new AskModule(addr, port, question, topic);
    }   

    public async Task<bool> ExecuteAsync()
    {
        string url = new ApiRouteBuilder(Address, Port).WithEndpoint(Endpoint).BuildUrl();
        Console.WriteLine(url);
        JsonContent content = JsonContent.Create<Payload>(new Payload { Topic = this.topic, Question = this.questions});

        HttpClient client = new();

        HttpResponseMessage res = await client.PostAsync(url, content);

        Console.WriteLine(await res.Content.ReadAsStringAsync());

        return true;
    }
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
