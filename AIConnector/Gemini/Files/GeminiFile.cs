using AIConnector.Common;

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
        MimeType = Path.GetExtension(FileName).AsGeminiMimeType();
        FileBytes = File.ReadAllBytes(this.filepath);
    }
}