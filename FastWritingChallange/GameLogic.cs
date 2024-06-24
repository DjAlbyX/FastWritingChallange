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

        internal async void play(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.Clear();
                timer = 10;
                word = list[Random.Next(0, list.Count)];

                Console.SetCursorPosition(0, 0);
                Console.Write($"score: {score}");

                var worker = new Thread(() =>
                {
                    while (timer > 0)
                    {
                        showTime();
                        timer--;
                        Thread.Sleep(1000);
                    }
                });
                worker.Start();

                Console.SetCursorPosition(5, 5);
                Console.Write($"{word}");
                handleInput();
            }
        }

        void handleInput()
        {
            Console.SetCursorPosition(20, 20);
            var input = Console.ReadLine();
            if (input == word && timer > 0)
            {
                score++;
                Console.WriteLine("Correct!");
            }
            else if (input != word && timer > 0)
            {
                Console.WriteLine("Wrong!");
            }
            Console.ReadKey();
        }

        private async void showTime()
        {
            Console.SetCursorPosition(20, 0);
            Console.Write($"timer: {timer}");
        }
    }
    }


