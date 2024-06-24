using FastWritingChallange;

var easy = new Easy();
var medium = new Medium();
var hard = new Hard();

var game = new GameLogic();

Run();
void Run()
{
    Console.Clear();
    Console.WriteLine($"1 for {easy.Name}");
    Console.WriteLine($"2 for {medium.Name}");
    Console.WriteLine($"3 for {hard.Name}");
    var input = Convert.ToInt32(Console.ReadLine());
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