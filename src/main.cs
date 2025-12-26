using System.Diagnostics;

//to add: linux or windows checks, and compatibility for both

class Program
{
    
    
    static bool IsExecutable(string fullPath)
    {
        try
        {
            var mode = File.GetUnixFileMode(fullPath);
            return (mode & UnixFileMode.UserExecute) != 0;
        }
        catch
        {
            return false;
        }
    }

    
    
    static string? FindExecutableInPath(string fileName)
    {
        var paths = Environment.GetEnvironmentVariable("PATH")?
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
        if (paths == null)
            return null;
        foreach (var path in paths)
        {
            var fullPath = Path.Combine(path, fileName);
            if (File.Exists(fullPath) && IsExecutable(fullPath))
                return fullPath;
        }
        return null;
    }

    static void RunProgram(string fullPath, string arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fullPath,
            Arguments = arguments,
            UseShellExecute = false

        };
        using var process = Process.Start(psi);
        process?.WaitForExit();
    }
    
    
    static void Main()
    {
        while (true)
        {
            Console.Write("$ "); 
            var consoleInput = Console.ReadLine();
            if (consoleInput == null) continue;
            var input = consoleInput.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var command = input[0];
            var message = string.Join(" ", input.Skip(1));

            switch (command)
            {
                case "type":
                    if (input.Length < 2) { Console.WriteLine("type: missing argument"); break; }
                    switch (input[1])
                    {
                        case "exit" or "quit" or "type" or "echo" or "pwd":
                            Console.WriteLine($"{input[1]} is a shell builtin");
                            break;
                        default: //assumes we are checking for paths, for now
                            var fullPath = FindExecutableInPath(input[1]);
                            Console.WriteLine(fullPath != null ? $"{input[1]} is {fullPath}" : $"{input[1]}: not found");
                            break;
                    }
                    break;
                case "exit":
                    Console.WriteLine("exit");
                    return;
                case "echo":
                    Console.WriteLine($"{message}");
                    break;
                case "pwd":
                    Console.WriteLine($"{Directory.GetCurrentDirectory()}");
                    break;
                case "cd":
                    if (input.Length < 2)
                    {
                        Console.WriteLine("cd: missing argument"); 
                        break;
                    }

                    if (!Directory.Exists(input[1]))
                    {
                        Console.WriteLine($"cd: {input[1]} No such file or directory");
                    }
                    Directory.SetCurrentDirectory(input[1]);
                    break;
                
                default: //now we assume the command is a program
                    var executable = FindExecutableInPath(command);
                    if (executable == null)
                    {
                        Console.WriteLine($"{command}: command not found");
                        break;
                    }
                    //giving the full path executable gave a test log error (it works, however console output is the path instead of executable name, so I am writing just the name for now, should be full executable normally 
                    RunProgram(command, message); //message is all words after first word
                    break;
            }
        }
    }
}

