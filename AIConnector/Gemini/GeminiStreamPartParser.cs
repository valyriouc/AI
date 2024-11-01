using System.Text;

namespace AIConnector.Gemini;

public static class GeminiStreamPartParser
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
            if (buffer.IsEmpty)
            {
                return buffer;
            }
            
            buffer = buffer.SkipWhitespace();
            buffer = ReadingHeader(buffer);
            
            buffer = ReadingContent(buffer, out string chunk);
            remains.Add(chunk);
        }
    }

    private static ReadOnlySpan<byte> ReadingHeader(ReadOnlySpan<byte> buffer)
    {
        if (buffer.Length < 5)
        {
            throw new GeminiException("Invalid header");
        }
        
        var actual = buffer[..5];
        bool isEqual = actual.IsTheSame([0x44, 0x61, 0x74, 0x61, 0x3a]);
        
        if (!isEqual)
        {
            throw new GeminiException("Invalid stream chunk");
        }

        return buffer[5..];
    }

    private static ReadOnlySpan<byte> ReadingContent(
        ReadOnlySpan<byte> buffer,
        out string chunk)
    {
        chunk = string.Empty;
        StringBuilder sb = new();

        byte[] newLine = buffer.GetNewLine();
        
        while (true)
        {
            if (buffer.Length < 4)
            {
                break;
            }
            
            if (buffer[..4].IsTheSame([..newLine, ..newLine]))
            {
                buffer = sb.Length == 0 ? 
                    ReadOnlySpan<byte>.Empty : 
                    buffer[4..];

                break;
            }
            
            sb.Append((char)buffer[0]);
            buffer = buffer[1..];
        }
        
        chunk = sb.ToString();
        return buffer;
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

    public static byte[] GetNewLine(this ReadOnlySpan<byte> self)
    {
        while (true)
        {
            if (self[0] == (byte)'\r' || self[0] == (byte)'\n')
            {
                break;
            }
            
            self = self[1..];
        }
        
        byte[] newLine = new byte[2];
        
        if (self.IsEmpty)
        { 
            return Array.Empty<byte>();
        }

        if (self[0] != (byte)'\n')
        {
            newLine[0] = (byte)'\n';
        }
        
        if (self[0] != (byte)'\r')
        {
            newLine[0] = (byte)'\r';
            
            if (self[1] != (byte)'\n')
            {
                newLine[1] = (byte)'\n';    
            }
        }

        return newLine;
    }

    public static bool IsTheSame(this ReadOnlySpan<byte> self, ReadOnlySpan<byte> other)
    {
        if (self.Length != other.Length)
        {
            return false;
        }

        for (int i = 0; i < self.Length; i++)
        {
            if (self[i] != other[i])
            {
                return false;
            }
        }

        return true;
    }
}