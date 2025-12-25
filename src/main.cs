class Program
{
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
                        default:
                            Console.WriteLine($"{input[1]}: command not found");
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

