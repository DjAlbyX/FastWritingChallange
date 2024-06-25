using FastWritingChallange;
class Program
{
    public static void Main()
    {
        Run();
    }

    static void Run()
    {
        var easy = new Easy();
        var medium = new Medium();
        var hard = new Hard();

        var game = new GameLogic();

        while (true)
        {
            Console.Clear();
            //new Mann().Mann1();
            Console.WriteLine($"1 for {easy.Name}");
            Console.WriteLine($"2 for {medium.Name}");
            Console.WriteLine($"3 for {hard.Name}");
            if (!int.TryParse(Console.ReadLine(), out int input) || input == 4)
            {
                break;
            }
            switch (input)
            {
                case 1:
                    game.play(easy.words);
                    break;

                case 2:
                    game.play(medium.words);
                    break;

                case 3:
                    game.play(hard.words);
                    break;

                default:
                    Console.Clear();
                    Run();
                    break;
            }
        }
    }
}