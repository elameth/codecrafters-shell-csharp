
public class AutoCompletionHandler : IAutoCompleteHandler
{
    public char[] Separators { get; set; } = "abcdefghijklmnopqrstuvwxyz".ToArray();

    private readonly string[] builtins = { "echo", "exit", "pwd", "cd", "type" };

    public string[] GetSuggestions(string text, int index)
    {
        return builtins
            .Where(b => b.StartsWith(text))
            .Select(b => b.Substring(text.Length) + " ")
            .ToArray();
    }
}
