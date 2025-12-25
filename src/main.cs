class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Write("$ "); 
            var command = Console.ReadLine();
            if (command != "exit")
                Console.WriteLine($"{command}: command not found");
            else
            {
                Console.WriteLine("exit");
                return;
            }
        }
        
        
    }
}

