using System.Text;

namespace AIConnector.Gemini;

internal static class GeminiStreamPartParser
{
    public static ReadOnlySpan<byte> Parse(
        ReadOnlySpan<byte> buffer, 
        out List<string> remains)
    {
        remains = new List<string>();
        
        // reading 'data:' entry 
        if (buffer.IsEmpty)
        {
            return ReadOnlySpan<byte>.Empty;
        }

        while (true)
        {
            buffer = buffer.SkipWhitespace();
            buffer = ReadingHeader(buffer);

            if (buffer.IsEmpty)
            {
                return buffer;
            }

            buffer = ReadingContent(buffer, out string chunk);
            remains.Add(chunk);
        }
    }

    private static ReadOnlySpan<byte> ReadingHeader(ReadOnlySpan<byte> buffer)
    {
        if (buffer[0..6] != [0x44, 0x61, 0x74, 0x61, 0x3a])
        {
            throw new GeminiException("Invalid stream chunk");
        }

        return buffer[6..];
    }

    private static ReadOnlySpan<byte> ReadingContent(
        ReadOnlySpan<byte> buffer, 
        out string chunk)
    {
        StringBuilder sb = new();

        while (true)
        {
            if (buffer[0..4] != [])
        }
    }
}

internal static class SpanExtensions
{
    public static ReadOnlySpan<byte> SkipWhitespace(this ReadOnlySpan<byte> self)
    {
        if (self.IsEmpty)
        {
            return ReadOnlySpan<byte>.Empty;
        }

        while (true)
        {
            if (self[0] != 0x20)
            {
                break;
            }

            self = self[1..];
        }

        return self;
    }
}