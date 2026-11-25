using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using galina_bot.Classes.Commands.TimeCommands;

namespace galina_bot.Classes.Commands.Dictionaries
{
    internal class CommandActions
    {
        public static void searchInBrowser(string[] words) //Функция поиска в интернете
        {
            int index = Array.IndexOf(words, "найди");
            var queryWords = words.Skip(index + 1);
            string searchQuery = string.Join(" ", queryWords);

            string url = "https://www.google.com/search?q=" + searchQuery;
            Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true // Вот эта строчка делает всю магию
            });

        }
        public static void setTimer(string[] words)
        {
            int index = Array.IndexOf(words, "на");
            var queryWords = words.Skip(index + 1);
            string timerWords = string.Join(" ", queryWords);
            Console.WriteLine(timerWords);

            string timeExpression = timeParserClass.timeParse(timerWords);
            Console.WriteLine($"получили такое уравнение:{timeExpression}");

            int computedTime = timeComputer.timeCompute(timeExpression);
            Console.WriteLine($"Нужен таймер на:{computedTime} секунды");


        }
    }
}
