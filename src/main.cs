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
                case "exit":
                    Console.WriteLine("exit");
                    return;
                case "echo":
                    Console.WriteLine($"{message} \n");
                    break;
                default:
                    Console.WriteLine($"{command}: command not found");
                    break;
            }
        }
        
        
    }
}

