using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FastWritingChallange
{
    internal class GameLogic
    {
        private int score;
        private int timer;
        Random Random = new();
        private string word;
        private readonly object lockObject = new();

        internal bool play(List<string> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                Console.Clear();
                timer = 10;
                word = list[Random.Next(0, list.Count)];

                Console.SetCursorPosition(0, 0);
                Console.Write($"score: {score}");

                using (var cts = new CancellationTokenSource())
                {
                    var worker = new Thread(() => TimerWorker(cts.Token));
                    worker.Start();

                    Console.SetCursorPosition(5, 5);
                    Console.Write($"{word}");

                    bool correct = HandleInput(cts);
                    worker.Join();

                    Console.SetCursorPosition(0, 8);
                    lock (lockObject)
                    {
                        if (correct)
                        {
                            score++;
                            Console.WriteLine("Correct!");
                        }
                        else
                        {
                            Console.WriteLine("Wrong!");
                        }
                    }
                    Thread.Sleep(1000);
                }
            }


            Console.Clear();
            Console.WriteLine($"Game Over! Your final score is {score}.");
            Console.WriteLine("Do you want to play again?(y/n)");

            var response = Console.ReadLine().Trim().ToLower();
            return response == "y";
        }

        private void TimerWorker(CancellationToken token)
        {
            while (timer > 0 && !token.IsCancellationRequested)
            {
                lock (lockObject)
                {
                    showTime();
                }
                Thread.Sleep(1000);
                lock (lockObject)
                {
                    timer--;
                }
            }
        }

        private bool HandleInput(CancellationTokenSource cts)
        {
            string input = string.Empty;
            DateTime endTime = DateTime.Now.AddSeconds(timer);

            Console.SetCursorPosition(5, 7);
            while (DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo KeyInfo = Console.ReadKey(intercept: true);
                    if (KeyInfo.Key == ConsoleKey.Enter)
                    {
                        cts.Cancel();
                        break;
                    }
                    input += KeyInfo.KeyChar;
                    lock (lockObject)
                    {
                        Console.SetCursorPosition(5, 7);
                        Console.Write(new string(' ', Console.WindowWidth - 5));
                        Console.SetCursorPosition(5, 7);
                        Console.Write(input);
                    }
                }
            }
            return input.Trim().Equals(word, StringComparison.OrdinalIgnoreCase);
        }
                
        private void showTime()
        {
            Console.SetCursorPosition(20, 0);
            Console.Write($"timer: {timer}");
        }
    }
}


