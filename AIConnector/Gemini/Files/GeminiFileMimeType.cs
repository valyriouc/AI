namespace AIConnector.Gemini.Files;

// TODO: Impl. the google file api for document working for gemini
// https://ai.google.dev/gemini-api/docs/document-processing?lang=rest
public enum GeminiFileMimeType
{
    Pdf,
    JavaScript,
    JavaScriptApp,
    Python,
    PythonApp,
    Text,
    Html,
    Css,
    Markdown,
    Csv,
    Xml,
    Rtf,
    Png,
    Jpeg,
    WebP,
    Heic,
    Heif
}

internal static class GeminiFileMimeTypeExtensions
{
    /// <summary>
    /// Converts the mime type into it's correct string representation
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static string AsString(this GeminiFileMimeType self)
    {
        return self switch
        {
            GeminiFileMimeType.Html => "text/html",
            GeminiFileMimeType.Pdf => "application/pdf",
            GeminiFileMimeType.JavaScript => "text/javascript",
            GeminiFileMimeType.JavaScriptApp => "application/x-javascript",
            GeminiFileMimeType.Python => "text/x-python",
            GeminiFileMimeType.PythonApp => "application/x-python",
            GeminiFileMimeType.Text => "text/plain",
            GeminiFileMimeType.Css => "text/css",
            GeminiFileMimeType.Markdown => "text/md",
            GeminiFileMimeType.Csv => "text/csv",
            GeminiFileMimeType.Xml => "text/xml",
            GeminiFileMimeType.Rtf => "text/rtf",
            GeminiFileMimeType.Png => "image/png",
            GeminiFileMimeType.Jpeg => "image/jpeg",
            GeminiFileMimeType.Heic => "image/heic",
            GeminiFileMimeType.Heif => "image/heif",
            _ => throw new NotImplementedException(),
        };
    }
}