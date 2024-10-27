using AIConnector.Gemini;
using AIConnector.Gemini.Files;

namespace AIConnector.Common;

internal static class StringExtensions
{
    public static GeminiFileMimeType AsGeminiMimeType(this string self)
    {
        return self switch
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
            _ => throw new GeminiException($"File type {self} not supported!")
        };
    }
}