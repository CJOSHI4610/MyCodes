namespace MyConsoleApp
{
    // This is a simple console application that prints "Hello, World!"
    class Program
    {
        static void Main(string[] args)
        {
            Consol.WriteLine("Hello, World!");
            // Introduce a syntax error by removing a semicolon
            Console.WriteLine("This line is missing a semicolon")
        }
    }
}