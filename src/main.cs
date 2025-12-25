class Program
{
    static void Main()
    {
        Console.Write("$ "); 
        var command = Console.ReadLine();
        if (command != null)
        {
            Console.WriteLine($"{command}: command not found");
        }
        
    }
}

