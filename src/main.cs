class Program
{
    /*
    static bool IsExecutable(string path)
    {
        var extension = Path.GetExtension(path);
        if (string.IsNullOrEmpty(extension))
            return false;

        var pathExtensions = Environment.GetEnvironmentVariable("PATHEXT");
        if (string.IsNullOrEmpty(pathExtensions))
            return false;

        return pathExtensions
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Any(e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
    }
    */

    static string? FindExecutableInPath(string fileName)
    {
        var paths = Environment.GetEnvironmentVariable("PATH")?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
        if (paths == null)
            return null;
        var pathExtensions = Environment.GetEnvironmentVariable("PATHEXT"); //raw
        if (string.IsNullOrEmpty(pathExtensions))
            return null;
        foreach (var path in paths)
        {
            var fullPath = Path.Combine(path, fileName);
            foreach (var pathExtension in pathExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries))
            {
                var fullPathWithExtension = fullPath + pathExtension;
                if (!File.Exists(fullPathWithExtension)) continue;
                return fullPath;
            }
        }
        return null;
    }
    static void Main()
    {
        while (true)
        {
            Console.Write("$ "); 
            var consoleInput = Console.ReadLine();
            if (consoleInput == null) continue;
            var input = consoleInput.Split(" ");
            var command = input[0];
            var message = string.Join(" ", input.Skip(1));

            switch (command)
            {
                case "type":
                    switch (input[1])
                    {
                        case "exit" or "quit" or "type" or "echo":
                            Console.WriteLine($"{input[1]} is a shell builtin");
                            break;
                        default: //assumes we are checking for paths, for now
                            var fullPath = FindExecutableInPath(input[1]);
                            if (fullPath != null)
                            {
                                Console.WriteLine($"{input[1]} is {fullPath}");
                            }
                            Console.WriteLine($"{input[1]}: not found");
                            break;
                    }
                    break;
                case "exit":
                    Console.WriteLine("exit");
                    return;
                case "echo":
                    Console.WriteLine($"{message}");
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;
            }
        }
    }
}

