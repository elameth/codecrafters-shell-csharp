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

    static void RunProgram(string fullPath, List<string> arguments)
    {
        var psi = new ProcessStartInfo
        {
            FileName = fullPath,
            UseShellExecute = false
        };
        foreach (var argument in arguments)
            psi.ArgumentList.Add(argument);
        
        using var process = Process.Start(psi);
        process?.WaitForExit();
    }
    
    
    
    static void Main()
    {
        //var redirectSet = new HashSet<string> { ">", "1>" };
        

        while (true)
        {
            Console.SetOut(Console.Out); //reset
            Console.Write("$ "); 
            var consoleInput = Console.ReadLine();
            if (consoleInput == null) continue;
            var tokenizedInput = TokenizationHandler.Tokenize(consoleInput);
            if (tokenizedInput == null)
            {
                Console.WriteLine($"{consoleInput}: input not found");
                continue;
            }
            var command = tokenizedInput[0];
            var message = string.Join(" ", tokenizedInput.Skip(1));
            var arguments = tokenizedInput.Skip(1).ToList();
          
            var redirectionIndex = tokenizedInput.FindIndex(t => t is ">" or "1>");
            //redirect needed or not
            if (redirectionIndex != -1)
            {
                if (redirectionIndex + 1 >= tokenizedInput.Count)
                {
                    Console.WriteLine("syntax error: expected filename after >");
                    continue;
                }

                var redirectFile = tokenizedInput[redirectionIndex + 1];
                arguments = tokenizedInput.Skip(redirectionIndex + 1).ToList(); //NEW ARGUMENTS FOR REDIRECTION
                var redirectWriter = new StreamWriter(redirectFile, append: false);
                redirectWriter.AutoFlush = true;
                Console.SetOut(redirectWriter);
            }

            switch (command)
            {
                case "type":
                    if (tokenizedInput.Count < 2) { Console.WriteLine("type: missing argument"); break; }
                    switch (tokenizedInput[1])
                    {
                        case "exit" or "quit" or "type" or "echo" or "pwd":
                            Console.WriteLine($"{tokenizedInput[1]} is a shell builtin");
                            break;
                        default: //assumes we are checking for paths, for now
                            var fullPath = FindExecutableInPath(tokenizedInput[1]);
                            Console.WriteLine(fullPath != null ? $"{tokenizedInput[1]} is {fullPath}" : $"{tokenizedInput[1]}: not found");
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
                    if (tokenizedInput.Count < 2)
                    {
                        Console.WriteLine("cd: missing argument"); 
                        break;
                    }

                    if (tokenizedInput[1] == "~")
                    {
                        var homePath = Environment.GetEnvironmentVariable("HOME");//this might be different in windows, check when adding windows
                        if (homePath != null)
                            Directory.SetCurrentDirectory(homePath);
                        break;
                    }
                    
                    if (!Directory.Exists(tokenizedInput[1]))
                    {
                        Console.WriteLine($"cd: {tokenizedInput[1]}: No such file or directory");
                        break;
                    }
                    Directory.SetCurrentDirectory(tokenizedInput[1]);
                    break;
                
                default: //now we assume the command is a program
                    var executable = FindExecutableInPath(command);
                    if (executable == null)
                    {
                        Console.WriteLine($"{command}: command not found");
                        break;
                    }
                    //giving the full path executable gave a test log error (it works, however console output is the path instead of executable name, so I am writing just the name for now, should be full executable normally 
                    RunProgram(command, arguments); //maybe a better way to skip first token?
                    break;
            }
        }
    }
}

