using galina_bot.Classes.Commands.TimeCommands;
using galina_bot.Classes.Commands.TimerUtils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
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
        public static void setTimer(string[] words) //Функция Таймера
        {
            int index = Array.IndexOf(words, "на"); 
            var queryWords = words.Skip(index + 1); //Удаляет все слова после "на". т.е. "поставь таймер на 15 минут" -> "15 минут"
            string timerWords = string.Join(" ", queryWords); //Склеивает массив в строку, которая дальше и обрабатывается

            string timeExpression = timeParserClass.timeParse(timerWords); //Парсит время, превращает ввод в уравнение, результат которого = секунды

            int computedTime = timeComputer.timeCompute(timeExpression); //Решает то самое строковое уравнение

            if (computedTime > 0) //computedTime = 0, если в прошлом методе произошла ошибка, так что если >0, значит все отработало нормально, и таймер можно запускать
            {
                TimerClass.setTimer(computedTime); //Запуск таймера
                SystemSounds.Asterisk.Play();
            }

        }
    }
}
