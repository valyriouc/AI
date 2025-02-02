using System.Text;

namespace OllamaMicrosoft;

public static class SourceCodeExtractor
{
    public static IEnumerable<string> Extract(string sourceCode)
    {
        ReadOnlySpan<char> span = sourceCode.AsSpan();
        List<string> result = new List<string>();
        
        while (!span.IsEmpty)
        {
            if (span[0] == '`')
            {
                span = span[1..];
                if (span.Length < 2)
                {
                    break;
                }

                if (span[0..2] is not "``")
                {
                    span = span[1..];
                    continue;   
                }
                
                span = span[2..];
            }
            else
            {
                span = span[1..];
                continue;
            }
            
            StringBuilder sb = new StringBuilder();
            
            while (!span.IsEmpty)
            {
                if (span[0] == '`')
                {
                    span = span[1..];
                    if (span.Length < 2)
                    {
                        break;
                    }
                    
                    if (span[0..2] is "``")
                    {
                        span = span[2..];
                        break;
                    }
                }
                
                sb.Append(span[0]);
                span = span[1..];
            }
            
            result.Add(sb.ToString()); 
        }

        return result;
    } 
}