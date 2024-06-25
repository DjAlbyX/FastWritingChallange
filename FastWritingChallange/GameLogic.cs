using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FastWritingChallange
{
    internal class GameLogic
    {
        private string input;
        private int score;
        private int timer;
        Random Random = new();
        private string word;
        private readonly object lockObject = new();
        private List <int> Index = new();


        internal void play(List<string> list)
        {

            for (int i = 0; i < list.Count; i++)
            {
                Console.Clear();
                timer = 10;
                word = list[GetIndex(list)];

                DisplayScore();
                DisplayWord();
                DisplayInputPrompt();
                showTime();

                using (var cts = new CancellationTokenSource())
                {
                    var worker = new Thread(() => TimerWorker(cts.Token));
                    worker.Start();

                    bool correct = HandleInput(cts);
                    worker.Join();

                    DisplayResult(correct);
                    Thread.Sleep(1000);
                }
            }
            EndGame(list);
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
            input = string.Empty;
            DateTime endTime = DateTime.Now.AddSeconds(timer);

            Console.SetCursorPosition(12, 5);
            //Console.Write(new string(' ', Console.WindowWidth - 12));
            //Console.SetCursorPosition(12, 5);
            
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
                    else if (KeyInfo.Key == ConsoleKey.Backspace && input.Length > 0) input = input.Remove(input.Length - 1);
                    else if(KeyInfo.Key != ConsoleKey.Backspace) input += KeyInfo.KeyChar;
                    
                    lock (lockObject)
                    {
                        Console.SetCursorPosition(12, 5);
                        Console.Write(new string(' ', Console.WindowWidth - 12));
                        Console.SetCursorPosition(12, 5);
                        Console.Write(input);
                    }
                }
            }
            return input.Trim().Equals(word, StringComparison.OrdinalIgnoreCase);
        }
                
        private void showTime()
        {
            lock (lockObject)
            {
                Console.SetCursorPosition(0, 1);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, 1);
                Console.Write($"timer: {timer}");
                //Console.SetCursorPosition(11, 5);
            }
        }

        private void DisplayScore()
        {
            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.Write($"score: {score}");
        }

        private void DisplayWord() 
        {
            Console.SetCursorPosition(0, 3);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 3);
            Console.Write($"{word}");
        }

        private void DisplayInputPrompt()
        {
            Console.SetCursorPosition(0, 5);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 5);
            Console.Write("Your Input: ");  
        }

        private void DisplayResult(bool correct)
        {
            Console.SetCursorPosition(0, 7);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 7);
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
        public int GetIndex(List <string> list) 
        {
            int rand;
            do
            {
                rand = Random.Next(0, list.Count);
            } while (Index.Contains(rand));

            Index.Add(rand);

            if(Index.Count == list.Count) 
            {
               Index.Clear();
            }
            return rand;
        }

        public void EndGame(List <string>list)
        {
            Console.Clear();
            Console.WriteLine($"Game Over! Your final score is {score}.");
            Console.WriteLine("Do you want to play the same level again?(y/n)");

            var response = Console.ReadLine().Trim().ToLower();
            switch (response)
            {
                case "y":
                    play(list);
                    break;

                case "n":
                    Program.Main();
                    break;

                default:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}


