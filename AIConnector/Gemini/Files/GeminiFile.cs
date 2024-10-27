namespace AIConnector.Gemini.Files;

public readonly struct GeminiFile
{
    private readonly string filepath;
    
    public string FileName { get; }

    public ReadOnlyMemory<byte> FileBytes { get; }
    
    public GeminiFileMimeType MimeType { get; }

    public int FileSize => FileBytes.Length;
    
    public GeminiFile(string filepath)
    {
        if (!File.Exists(filepath))
        {
            throw new FileNotFoundException("File not found", filepath);
        }
        
        this.filepath = filepath;
        FileName = Path.GetFileName(filepath);
        MimeType = DetermineFileType(FileName);
        FileBytes = File.ReadAllBytes(this.filepath);
    }

    private static GeminiFileMimeType DetermineFileType(string filename)
    {
        string extension = Path.GetExtension(filename);

        return extension switch
        {
            ".md" => GeminiFileMimeType.Markdown,
            ".pdf" => GeminiFileMimeType.Pdf,
            ".txt" => GeminiFileMimeType.Text,
            ".js" => GeminiFileMimeType.JavaScript,
            ".py" => GeminiFileMimeType.Python,
            ".html" or ".htm" or ".htmlx" => GeminiFileMimeType.Html,
            ".css" => GeminiFileMimeType.Css,
            ".xml" => GeminiFileMimeType.Xml,
            ".rtf" => GeminiFileMimeType.Rtf,
            ".csv" => GeminiFileMimeType.Csv,
            _ => throw new GeminiException($"File type {extension} not supported!")
        };
    }
}