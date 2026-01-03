
public class AutoCompletionHandler : IAutoCompleteHandler
{
    public char[] Separators { get; set; } = "abcdefghijklmnopqrstuvwxyz".ToArray();

    private readonly string[] builtins = { "echo", "exit", "pwd", "cd", "type" };

    public string[] GetSuggestions(string text, int index)
    {
        var matches = builtins.Where(x => x.StartsWith(text)).ToArray();
        if (matches.Length == 0)
        {
            Console.Write("\x07");
            return Array.Empty<string>();
        }
        
        return matches.Select(b => b.Substring(text.Length) + " ").ToArray();
    }
}
