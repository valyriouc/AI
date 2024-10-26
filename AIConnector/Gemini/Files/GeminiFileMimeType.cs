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
    Heif,
    Mp4,
    Mpeg,
    Mov,
    Avi,
    XFlv,
    Mpg,
    WebM,
    Wmv,
    Gpp3,
    Wav,
    Mp3,
    Aiff,
    Aac,
    Ogg,
    Flac
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
            GeminiFileMimeType.WebP => "image/webp",
            GeminiFileMimeType.Heic => "image/heic",
            GeminiFileMimeType.Heif => "image/heif",
            GeminiFileMimeType.Mp4 => "video/mp4",
            GeminiFileMimeType.Mpeg => "video/mpeg",
            GeminiFileMimeType.Mov => "video/mov",
            GeminiFileMimeType.Avi => "video/avi",
            GeminiFileMimeType.XFlv => "video/x-flv",
            GeminiFileMimeType.Mpg => "video/mpg",
            GeminiFileMimeType.WebM => "video/webm",
            GeminiFileMimeType.Wmv => "video/wmv",
            GeminiFileMimeType.Gpp3 => "video/3gpp",
            GeminiFileMimeType.Wav => "audio/wav",
            GeminiFileMimeType.Mp3 => "audio/mp3",
            GeminiFileMimeType.Aiff => "audio/aiff",
            GeminiFileMimeType.Aac => "audio/aac",
            GeminiFileMimeType.Ogg => "audio/ogg",
            GeminiFileMimeType.Flac => "audio/flac",
            _ => throw new NotImplementedException(),
        };
    }
}