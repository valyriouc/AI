namespace AIConnector.Gemini;

public class GeminiRequestContent(
   List<IGeminiRequestPart> parts,
   string role)
{
   public List<IGeminiRequestPart> Parts { get; } = parts;

   public string Role { get; } = role;
}